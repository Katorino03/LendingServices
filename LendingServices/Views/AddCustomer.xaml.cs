using System;
using System.Windows;
using System.Windows.Controls;

namespace LendingServices.Views
{
    public partial class AddCustomer : Page
    {
        public AddCustomer()
        {
            InitializeComponent();
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            string accountNumber = txtAccount.Text;
            string customerName = txtName.Text;
            string address = txtAddress.Text;
            string contact = txtContact.Text;
            string loanAmount = txtLoan.Text;
            string dailyPayment = txtDaily.Text;
            string terms = txtTerms.Text;
            DateTime? loanedDate = dpLoaned.SelectedDate;
            string dueDate = txtDue.Text;

            MessageBox.Show($"Customer '{customerName}' added!\n" +
                            $"Account: {accountNumber}\n" +
                            $"Loan: ₱{loanAmount}\n" +
                            $"Due: {dueDate}",
                            "Success",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtAccount.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtContact.Text = "";
            txtLoan.Text = "";
            txtDaily.Text = "";
            txtTerms.Text = "";
            dpLoaned.SelectedDate = null;
            txtDue.Text = "";
        }

        private void dpLoaned_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ComputeDueDate();
        }

        private void txtTerms_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComputeDueDate();
        }

        private void ComputeDueDate()
        {
            if (dpLoaned.SelectedDate.HasValue && int.TryParse(txtTerms.Text, out int days))
            {
                DateTime due = dpLoaned.SelectedDate.Value.AddDays(days);
                txtDue.Text = due.ToString("dd/MM/yyyy");
            }
            else
            {
                txtDue.Text = "";
            }
        }
    }
}
