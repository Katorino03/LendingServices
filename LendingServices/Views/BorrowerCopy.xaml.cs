using System.Windows;
using System.Windows.Controls;
using LendingServices.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LendingServices.Views
{
    public partial class BorrowerCopy : Page
    {
        private readonly BorrowerCopyViewModel _viewModel;

        public BorrowerCopy()
        {
            InitializeComponent();

            _viewModel = App.ServiceProvider.GetRequiredService<BorrowerCopyViewModel>();
            DataContext = _viewModel;

            this.Loaded += async (s, e) => await _viewModel.LoadCustomerListAsync();
        }

        private void PrintNow_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                btnPrint.Visibility = Visibility.Hidden;
                printDialog.PrintVisual(this, "Borrower Ledger");
                btnPrint.Visibility = Visibility.Visible;
            }
        }
    }
}