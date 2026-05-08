using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LendingServices.DTOs;
using LendingServices.Repositories;

namespace LendingServices.ViewModels
{
    public partial class DailyCollectionViewModel : ObservableObject
    {
        private readonly ILoanRepository _loanRepository;

        public ObservableCollection<DailyCollectionDTO> Customers { get; } = new();

        [ObservableProperty]
        private decimal _totalCollectibles;

        [ObservableProperty]
        private decimal _totalCollection;

        [ObservableProperty]
        private decimal _netCollection;

        public DailyCollectionViewModel(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
            Customers.CollectionChanged += (s, e) => ComputeTotals();
        }

        public async Task LoadDataAsync()
        {
            var activeLoans = await _loanRepository.GetDailyCollectionListAsync();

            Customers.Clear();
            foreach (var loan in activeLoans)
            {
                loan.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(DailyCollectionDTO.AmountPaidToday))
                    {
                        ComputeTotals();
                    }
                };
                Customers.Add(loan);
            }
            ComputeTotals();
        }

        private void ComputeTotals()
        {
            TotalCollectibles = Customers.Sum(c => c.DailyPayment);
            TotalCollection = Customers.Sum(c => c.AmountPaidToday);
            NetCollection = TotalCollection;
        }

        [RelayCommand]
        private async Task RecordCollectionAsync()
        {
            try
            {
                await _loanRepository.SaveDailyCollectionsAsync(Customers);

                MessageBox.Show("Daily collection recorded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void ClearAll()
        {
            foreach (var c in Customers)
            {
                c.AmountPaidToday = 0;
            }
        }
    }
}