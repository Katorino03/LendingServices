using CommunityToolkit.Mvvm.ComponentModel;
using LendingServices.Data;
using LendingServices.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LendingServices.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly AngCoolDbContext _context;

        [ObservableProperty]
        private decimal _totalOutstanding;

        [ObservableProperty]
        private int _overdueCustomersCount;

        public record OverdueCustomerInfo(string Name, decimal Balance);

        [ObservableProperty]
        private ObservableCollection<OverdueCustomerInfo> _overdueCustomersList = new();

        [ObservableProperty]
        private int _dueThisWeekCount;

        [ObservableProperty]
        private decimal _dailyReceipts;

        [ObservableProperty]
        private ObservableCollection<ISeries> _receiptsSeries = new();

        [ObservableProperty]
        private ObservableCollection<Axis> _xAxes = new();

        public DashboardViewModel(AngCoolDbContext context)
        {
            _context = context;
        }

        public async Task LoadDataAsync()
        {
            try
            {
                var loans = await _context.Loans
                    .Include(l => l.Customer)
                    .Include(l => l.Payments)
                    .ToListAsync();

                TotalOutstanding = loans
                    .Where(l => l.IsActive)
                    .Sum(l => l.TotalAmountToPay - l.Payments.Sum(p => p.AmountPaid));

                var today = DateTime.Today;

               
                var overdueCustomers = loans
                    .Where(l => l.IsActive && l.DueDate < today && (l.TotalAmountToPay - l.Payments.Sum(p => p.AmountPaid)) > 0)
                    .Select(l => new OverdueCustomerInfo(
                        l.Customer.Name,
                        l.TotalAmountToPay - l.Payments.Sum(p => p.AmountPaid)
                    ))
                    .ToList();

                OverdueCustomersCount = overdueCustomers.Count;
                OverdueCustomersList = new ObservableCollection<OverdueCustomerInfo>(overdueCustomers);

                var nextWeek = today.AddDays(7);
                DueThisWeekCount = loans
                    .Where(l => l.IsActive && l.DueDate >= today && l.DueDate <= nextWeek && (l.TotalAmountToPay - l.Payments.Sum(p => p.AmountPaid)) > 0)
                    .Select(l => l.CustomerId)
                    .Distinct()
                    .Count();

                var startOfDay = today;
                var endOfDay = today.AddDays(1).AddTicks(-1);

                DailyReceipts = await _context.Payments
                    .Where(p => p.PaymentDate >= startOfDay && p.PaymentDate <= endOfDay)
                    .SumAsync(p => p.AmountPaid);

                
                var last7Days = Enumerable.Range(0, 7).Select(offset => today.AddDays(-6 + offset)).ToList();
                var chartData = new double[7];
                var labels = new string[7];

                for (int i = 0; i < 7; i++)
                {
                    var dayStart = last7Days[i];
                    var dayEnd = dayStart.AddDays(1).AddTicks(-1);
                    var dailySum = await _context.Payments
                        .Where(p => p.PaymentDate >= dayStart && p.PaymentDate <= dayEnd)
                        .SumAsync(p => p.AmountPaid);

                    chartData[i] = (double)dailySum;
                    labels[i] = dayStart.ToString("ddd"); // Mon, Tue, etc.
                }

                
                ReceiptsSeries = new ObservableCollection<ISeries>
{
                    new LineSeries<double>
                    {
                        Values = chartData,
                        Name = "Daily Receipts",
                        Fill = new SolidColorPaint(new SKColor(59, 130, 246, 50)), // Light Blue fill
                        Stroke = new SolidColorPaint(new SKColor(59, 130, 246), 4), // Blue line, thick
                        GeometrySize = 10, // Kadako sa mga dots
                        GeometryStroke = new SolidColorPaint(new SKColor(255, 255, 255), 2),
                        LineSmoothness = 1 // 1 = Smooth curve, 0 = straight lines
                    }
                };

                XAxes = new ObservableCollection<Axis>
                {
                    new Axis
                    {
                        Labels = labels
                    }
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Dashboard Data Load Error: {ex.Message}");
            }
        }
    }
}