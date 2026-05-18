using System.Windows;

namespace LendingServices.Views
{
    public partial class LoginWindow : Window
    {
        private bool isPasswordVisible = false;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // SAMPLE LOGIN VALIDATION
            if (username == "admin" && password == "1234")
            {
               
                MainWindow main = new MainWindow();
                main.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.",
                                "Login Failed",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            // Simple toggle message (for demo purposes)
            if (!isPasswordVisible)
            {
                MessageBox.Show("Password visibility toggle sample.");
                isPasswordVisible = true;
            }
            else
            {
                isPasswordVisible = false;
            }
        }
    }
}
