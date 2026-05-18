using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LendingServices.Models;
using LendingServices.Repositories;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace LendingServices.ViewModels
{
    public partial class AddCustomerViewModel : ObservableObject
    {
        private readonly ILoanRepository _loanRepository;
        [ObservableProperty] private string _accountNo = string.Empty;
        [ObservableProperty] private string _customerName = string.Empty;
        [ObservableProperty] private string _address = string.Empty;
        [ObservableProperty] private string _contactNo = string.Empty;
        [ObservableProperty] private decimal _loanAmount;
        [ObservableProperty] private decimal _dailyPayment;
        [ObservableProperty] private int _terms;
        [ObservableProperty] private DateTime _dateLoaned = DateTime.Now;
        [ObservableProperty] private DateTime? _dueDate;
        [ObservableProperty] private string _selectedLoanType = string.Empty;

        public AddCustomerViewModel(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
            CalculateDueDate();
        }

        partial void OnTermsChanged(int value) => CalculateDueDate();
        partial void OnDateLoanedChanged(DateTime value) => CalculateDueDate();
        private void CalculateDueDate()
        {
            if (Terms > 0)
            {
                DueDate = DateLoaned.AddDays(Terms);
            }
            else
            {
                DueDate = null;
            }
        }

        [RelayCommand]
        private async Task SaveCustomerAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CustomerName) || LoanAmount <= 0)
                {
                    MessageBox.Show("Please fill in all required fields.", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                    return;
                }

                var newCustomer = new Customer
                {
                    AccountNo = this.AccountNo,
                    Name = this.CustomerName,
                    Address = this.Address,
                    ContactNo = this.ContactNo
                };

                var newLoan = new Loan
                {
                    PrincipalAmount = this.LoanAmount,
                    TotalAmountToPay = this.LoanAmount,
                    DailyPayment = this.DailyPayment,
                    TermsInDays = this.Terms,
                    DateLoaned = this.DateLoaned,
                    DueDate = this.DueDate ?? DateTime.Now,
                    IsActive = true,
                    LoanType = string.IsNullOrEmpty(this.SelectedLoanType) ? null : this.SelectedLoanType
                };

                await _loanRepository.AddNewCustomerAndLoanAsync(newCustomer, newLoan);
                MessageBox.Show("Customer and Loan added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void ClearForm()
        {
            AccountNo = string.Empty;
            CustomerName = string.Empty;
            Address = string.Empty;
            ContactNo = string.Empty;
            LoanAmount = 0;
            DailyPayment = 0;
            Terms = 0;
            DateLoaned = DateTime.Now;
            SelectedLoanType = string.Empty;
        }
    }
}