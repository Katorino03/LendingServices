using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LendingServices.DTOs;
using LendingServices.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LendingServices.ViewModels
{
    public partial class CustomerListViewModel : ObservableObject
    {
        private readonly ILoanRepository _loanRepository;
        private List<CustomerListItemDTO> _allCustomers = new();

        [ObservableProperty]
        private ObservableCollection<CustomerListItemDTO> _customers = new();

        [ObservableProperty]
        private int _totalCustomers;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private int _selectedSortIndex = 0;

        [ObservableProperty]
        private int _selectedFilterIndex = 0;

        [ObservableProperty]
        private bool _isLoading;

        public CustomerListViewModel(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        partial void OnSearchTextChanged(string value) => ApplyFilters();
        partial void OnSelectedSortIndexChanged(int value) => ApplyFilters();
        partial void OnSelectedFilterIndexChanged(int value) => ApplyFilters();

        public async Task LoadDataAsync()
        {
            if (IsLoading) return;

            IsLoading = true;

            try
            {
                await Task.Delay(300);

                var data = await _loanRepository.GetCustomerListAsync();
                _allCustomers = data?.ToList() ?? new List<CustomerListItemDTO>();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Data Load Error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ApplyFilters()
        {
            IEnumerable<CustomerListItemDTO> filteredList = _allCustomers;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredList = filteredList.Where(c =>
                    (c.Name != null && c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (c.AccountNo != null && c.AccountNo.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            }

            if (SelectedFilterIndex == 1)
                filteredList = filteredList.Where(c => c.Status == "Active");
            else if (SelectedFilterIndex == 2)
                filteredList = filteredList.Where(c => c.Status == "Overdue");

            filteredList = SelectedSortIndex == 0
                ? filteredList.OrderBy(c => c.Name)
                : filteredList.OrderByDescending(c => c.Name);

            Customers = new ObservableCollection<CustomerListItemDTO>(filteredList);
            TotalCustomers = Customers.Count;
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            SearchText = string.Empty;
            SelectedSortIndex = 0;
            SelectedFilterIndex = 0;
            await LoadDataAsync();
        }
    }
}