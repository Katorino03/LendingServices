using LendingServices.Models;
using LendingServices.Repositories;
using System;
using System.Windows;
using System.Windows.Input;

namespace LendingServices.Views
{
    public partial class ForgotPasswordWindow : Window
    {
        private readonly IUserRepository _userRepository;
        private UserAccount? _foundUser;

        public ForgotPasswordWindow(IUserRepository userRepository)
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
            this.Close();
        }

        private async void FindAccount_Click(object sender, RoutedEventArgs e)
        {
            string username = txtRecoveryUsername.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter a username.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _foundUser = await _userRepository.GetUserByUsernameAsync(username);

            if (_foundUser != null)
            {
                txtSecurityQuestion.Text = _foundUser.SecurityQuestion;

                Step1Panel.Visibility = Visibility.Collapsed;
                Step2Panel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("We couldn't find an account with that username.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (_foundUser == null) return;

            string answer = txtSecurityAnswer.Text.Trim();
            string newPassword = txtNewPassword.Password;

            if (string.IsNullOrWhiteSpace(answer) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Please fill in both the security answer and a new password.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (answer.Equals(_foundUser.SecurityAnswer, StringComparison.OrdinalIgnoreCase))
            {
                _foundUser.Password = newPassword;

                await _userRepository.UpdateUserAsync(_foundUser);

                MessageBox.Show("Your password has been successfully reset! You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect security answer. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtSecurityAnswer.Clear();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _foundUser = null;
            txtSecurityAnswer.Clear();
            txtNewPassword.Clear();

            Step2Panel.Visibility = Visibility.Collapsed;
            Step1Panel.Visibility = Visibility.Visible;
        }
    }
}