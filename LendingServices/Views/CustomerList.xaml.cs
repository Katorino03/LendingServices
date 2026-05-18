using System.Windows;
using System.Windows.Controls;
using LendingServices.DTOs;
using LendingServices.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LendingServices.Views
{
    public partial class CustomerList : Page
    {
        private readonly CustomerListViewModel _viewModel;

        public CustomerList()
        {
            InitializeComponent();

            _viewModel = App.ServiceProvider.GetRequiredService<CustomerListViewModel>();
            DataContext = _viewModel;

            Loaded += async (s, e) => await _viewModel.LoadDataAsync();
        }

        private void ViewLedger_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is CustomerListItemDTO selectedCustomer)
            {
                MessageBox.Show($"Selected: {selectedCustomer.Name}\nBalance: ₱{selectedCustomer.Balance:N2}",
                                "Ledger Clicked",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }
    }
}