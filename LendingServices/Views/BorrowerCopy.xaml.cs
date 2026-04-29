using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LendingServices.Models; 

namespace LendingServices.Views
{
    public partial class BorrowerCopy : Page
    {
        public BorrowerCopy()
        {
            InitializeComponent();
        }
        public BorrowerCopy(CustomerList.Customer customer, ObservableCollection<LedgerEntry> ledgerEntries)
        {
            InitializeComponent();
            this.DataContext = customer;
            dgLedger.ItemsSource = ledgerEntries;
        }

        private void PrintNow_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                pd.PrintVisual(this, "Borrower Ledger");
            }
        }
    }
}
