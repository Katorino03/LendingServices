using CommunityToolkit.Mvvm.ComponentModel;
using LendingServices.Data;
using LendingServices.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

        [ObservableProperty]
        private int _dueThisWeekCount;

        [ObservableProperty]
        private decimal _dailyReceipts;

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

                OverdueCustomersCount = loans
                    .Where(l => l.IsActive && l.DueDate < today && (l.TotalAmountToPay - l.Payments.Sum(p => p.AmountPaid)) > 0)
                    .Select(l => l.CustomerId)
                    .Distinct()
                    .Count();

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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Dashboard Data Load Error: {ex.Message}");
            }
        }
    }
}