using LendingServices.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LendingServices
{
    public partial class UserManagementWindow : Window
    {
        private readonly IUserRepository _userRepository;
        private string _currentMode = "Profile";
        private string _originalUsername;

        public UserManagementWindow(string currentUsername)
        {
            InitializeComponent();
            _userRepository = App.ServiceProvider.GetRequiredService<IUserRepository>();
            _originalUsername = currentUsername;

            LoadCurrentUserData();
        }

        private async void LoadCurrentUserData()
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(_originalUsername);
                if (user != null)
                {
                    txtFullName.Text = user.FullName;
                    txtUsername.Text = user.Username;
                }

                txtUsername.Text = _originalUsername;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load user data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseModal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Tab_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                _currentMode = btn.Tag.ToString() ?? "Profile";

                if (_currentMode == "Profile")
                {
                    txtSectionTitle.Text = "Edit My Information";
                    lblPassword.Text = "Change Password";
                    btnSubmit.Content = "Save Changes";
                    btnSubmit.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2563EB"));
                    LoadCurrentUserData();
                }
                else
                {
                    txtSectionTitle.Text = "Register New Account";
                    lblPassword.Text = "Set Initial Password";
                    btnSubmit.Content = "Create Account";
                    btnSubmit.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669"));

                    txtFullName.Clear();
                    txtUsername.Clear();
                    txtPassword.Clear();
                    txtConfirmPassword.Clear();
                    txtSecurityAnswer.Clear();
                    cmbSecurityQuestion.SelectedIndex = -1;
                }
            }
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password) ||
                cmbSecurityQuestion.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(txtSecurityAnswer.Text))
            {
                MessageBox.Show("Please fill in all fields before saving.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                MessageBox.Show("Passwords do not match. Please try again.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedItem = cmbSecurityQuestion.SelectedItem as ComboBoxItem;
            string securityQuestion = selectedItem?.Content?.ToString() ?? "Default Question";
            string normalizedAnswer = txtSecurityAnswer.Text?.ToLower().Replace(" ", "") ?? "";

            try
            {
                if (_currentMode == "Profile")
                {
                    await _userRepository.UpdateUserAsync(
                        _originalUsername,
                        txtUsername.Text,
                        txtFullName.Text,
                        txtPassword.Password,
                        securityQuestion,
                        normalizedAnswer
                    );

                    MessageBox.Show("Your profile has been updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    _originalUsername = txtUsername.Text;
                    this.Close();
                }
                else
                {
                    bool userExists = await _userRepository.UserExistsAsync(txtUsername.Text);
                    if (userExists)
                    {
                        MessageBox.Show("This username is already taken. Please choose another.", "Username Taken", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    await _userRepository.CreateUserAsync(txtUsername.Text, txtFullName.Text, txtPassword.Password, securityQuestion, normalizedAnswer);

                    MessageBox.Show("New account created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    txtFullName.Clear();
                    txtUsername.Clear();
                    txtPassword.Clear();
                    txtConfirmPassword.Clear();
                    txtSecurityAnswer.Clear();
                    cmbSecurityQuestion.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving to the database: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}