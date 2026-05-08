using LendingServices;
using LendingServices.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace LendingServices
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Dashboard());
            SetActive(btnDashboard);
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Dashboard());
            SetActive(btnDashboard);
        }

        private void CustomerList_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CustomerList());
            SetActive(btnCustomerList);
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AddCustomer());
            SetActive(btnAddCustomer);
        }

        private void DailyCollection_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DailyCollection());
            SetActive(btnDailyCollection);
        }

        private void BorrowerCopy_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BorrowerCopy());
            SetActive(btnBorrowerCopy);
        }

        private void SetActive(Button activeBtn)
        {
            btnDashboard.Background = Brushes.Transparent;
            btnCustomerList.Background = Brushes.Transparent;
            btnAddCustomer.Background = Brushes.Transparent;
            btnDailyCollection.Background = Brushes.Transparent;
            btnBorrowerCopy.Background = Brushes.Transparent;
            activeBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8F0FF"));
        }
    }
    
}

