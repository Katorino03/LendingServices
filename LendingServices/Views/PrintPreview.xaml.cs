using LendingServices.DTOs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation; // Siguroha nga naa ni para sa NavigationService

namespace LendingServices.Views
{
    public partial class PrintPreview : Page
    {
        public PrintPreview(ObservableCollection<DailyCollectionDTO> customers)
        {
            InitializeComponent();

            var left = new ObservableCollection<DailyCollectionDTO>();
            var right = new ObservableCollection<DailyCollectionDTO>();

            for (int i = 0; i < customers.Count; i++)
            {
                if (i % 2 == 0)
                    left.Add(customers[i]);
                else
                    right.Add(customers[i]);
            }

            dgLeft.ItemsSource = left;
            dgRight.ItemsSource = right;

            decimal collectibles = customers.Sum(c => c.DailyPayment);
            decimal total = customers.Sum(c => c.AmountPaidToday);

            txtCollectibles.Text = $"COLLECTIBLES: ₱ {collectibles}";
            txtNet.Text = $"NET COLLECTION: ₱ {total}";
            txtDate.Text = $"DATE: {System.DateTime.Now:MMMM dd, yyyy}";
        }

        private void PrintNow_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                pd.PrintVisual(printArea, "Daily Collection");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null && this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack(); 
            }
        }
    }
}