using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LendingServices.Models;

namespace LendingServices.Views
{
    public partial class DailyCollection : Page
    {
        private ObservableCollection<Customer> customers;


        public DailyCollection()
        {
            InitializeComponent();
            //replace ni na data, pag naka database kenemerut na ha
            customers = new ObservableCollection<Customer>
            {
                new Customer{AccountNo=407, Name="JELBERT ANTONIO", DailyPayment=100, Balance=1200, AmountPaidToday=0},
                new Customer{AccountNo=408, Name="MARIA CLARA", DailyPayment=120, Balance=2400, AmountPaidToday=0},
                new Customer{AccountNo=409, Name="PEDRO SANTOS", DailyPayment=130, Balance=650, AmountPaidToday=0}
            };

            dgCustomers.ItemsSource = customers;
            ComputeTotals();
        }

        private void AmountChanged(object sender, TextChangedEventArgs e)
        {
            ComputeTotals();
        }

        private void ComputeTotals()
        {
            decimal totalCollectibles = customers.Sum(c => c.DailyPayment);
            decimal totalCollection = customers.Sum(c => c.AmountPaidToday);
            decimal netCollection = totalCollection; 

            txtCollectibles.Text = $"₱ {totalCollectibles}";
            txtTotalCollection.Text = $"₱ {totalCollection}";
            txtNetCollection.Text = $"₱ {netCollection}";
        }

        private void RecordCollection_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Daily collection recorded successfully!",
                            "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var c in customers)
            {
                c.AmountPaidToday = 0;
            }
            dgCustomers.Items.Refresh();
            ComputeTotals();
        }

        private void PreviewPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintPreview previewPage = new PrintPreview(customers);

            this.NavigationService?.Navigate(previewPage);
        }


    }
}
