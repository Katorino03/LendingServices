using LendingServices;
using LendingServices.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Threading;

namespace LendingServices
{
    public partial class MainWindow : Window
    {
        public bool RequiresPasswordChange { get; set; } = false;
        public string CurrentUsername { get; set; } = string.Empty;

        public MainWindow()
        {
            InitializeComponent();

            StartClock();
            MainFrame.Navigate(new Dashboard());
            SetActive(btnDashboard);
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (RequiresPasswordChange)
            {
                MessageBox.Show("Security Alert: You are using the default credentials. Please update your profile and security questions immediately.",
                                "Update Required", MessageBoxButton.OK, MessageBoxImage.Warning);

                OpenUserManagement();
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            OpenUserManagement();
        }

        private void OpenUserManagement()
        {
            DimOverlay.Visibility = Visibility.Visible;

            var fadeIn = new System.Windows.Media.Animation.DoubleAnimation(0, 0.4, TimeSpan.FromSeconds(0.3));
            DimOverlay.BeginAnimation(Border.OpacityProperty, fadeIn);

            var userSettingsWindow = new UserManagementWindow(CurrentUsername);
            userSettingsWindow.Owner = this;

            userSettingsWindow.ShowDialog();

            var fadeOut = new System.Windows.Media.Animation.DoubleAnimation(0.4, 0, TimeSpan.FromSeconds(0.3));
            fadeOut.Completed += (s, ev) => DimOverlay.Visibility = Visibility.Collapsed;
            DimOverlay.BeginAnimation(Border.OpacityProperty, fadeOut);
        }

        private void StartClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                txtTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
                txtDate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
            };
            timer.Start();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Logout Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var loginWindow = App.ServiceProvider.GetRequiredService<LoginWindow>();
                loginWindow.Show();
                this.Close();
            }
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
            btnDashboard.Tag = null;
            btnCustomerList.Tag = null;
            btnAddCustomer.Tag = null;
            btnDailyCollection.Tag = null;
            btnBorrowerCopy.Tag = null;

            if (activeBtn != null)
            {
                activeBtn.Tag = "Active";
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit the application?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}