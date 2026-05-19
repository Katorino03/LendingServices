using LendingServices.Repositories;
using System;
using System.Windows;
using System.Windows.Input;

namespace LendingServices.Views
{
    public partial class LoginWindow : Window
    {
        private readonly IUserRepository _userRepository;
        private bool isPasswordVisible = false;

        public LoginWindow(IUserRepository userRepository)
        {
            InitializeComponent();
            _userRepository = userRepository;
            this.MouseDown += Window_MouseDown;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = isPasswordVisible ? txtPasswordReveal.Text : txtPassword.Password;

            bool isValid = await _userRepository.ValidateUserAsync(username, password);

            if (isValid)
            {
                MainWindow main = new MainWindow();
                main.CurrentUsername = username;

                if (username == "admin" && password == "password123")
                {
                    main.RequiresPasswordChange = true;
                }

                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            if (!isPasswordVisible)
            {
                txtPasswordReveal.Text = txtPassword.Password;
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordReveal.Visibility = Visibility.Visible;
                btnShowPassword.Content = "❌";
                isPasswordVisible = true;

                txtPasswordReveal.Focus();
            }
            else
            {
                txtPassword.Password = txtPasswordReveal.Text;
                txtPasswordReveal.Visibility = Visibility.Collapsed;
                txtPassword.Visibility = Visibility.Visible;
                btnShowPassword.Content = "👁";
                isPasswordVisible = false;

                txtPassword.Focus();
            }
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow(_userRepository);
            forgotPasswordWindow.ShowDialog();
        }
    }
}