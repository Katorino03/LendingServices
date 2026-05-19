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
        [ObservableProperty] private double _interestRate;
        [ObservableProperty] private int _terms;
        [ObservableProperty] private DateTime _dateLoaned = DateTime.Now;
        [ObservableProperty] private string _selectedLoanType = string.Empty;

        [ObservableProperty] private decimal _dailyPayment;
        [ObservableProperty] private DateTime? _dueDate;

        [ObservableProperty] private decimal _totalCollectible;

        public AddCustomerViewModel(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
            _dateLoaned = DateTime.Now;
            _dueDate = null;
        }

        #region Automatic Calculation Logic

        partial void OnTermsChanged(int value) => CalculateFinancials();
        partial void OnLoanAmountChanged(decimal value) => CalculateFinancials();
        partial void OnInterestRateChanged(double value) => CalculateFinancials();

        private void CalculateFinancials()
        {
            if (Terms > 0 && LoanAmount > 0)
            {
                decimal interestAmount = LoanAmount * (decimal)(InterestRate / 100);
                TotalCollectible = LoanAmount + interestAmount;

                DailyPayment = TotalCollectible / Terms;
            }
            else
            {
                TotalCollectible = 0;
                DailyPayment = 0;
            }
        }

        #endregion

        [RelayCommand]
        private async Task SaveCustomerAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CustomerName) || LoanAmount <= 0 || Terms <= 0 || DueDate == null)
                {
                    MessageBox.Show("Please provide a valid Customer Name, Loan Amount, Terms, and pick a Due Date.",
                        "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    TotalAmountToPay = this.TotalCollectible,
                    DailyPayment = this.DailyPayment,
                    TermsInDays = this.Terms,
                    DateLoaned = this.DateLoaned,
                    DueDate = this.DueDate.Value,
                    IsActive = true,
                    LoanType = string.IsNullOrEmpty(this.SelectedLoanType) ? null : this.SelectedLoanType,
                    InterestRate = this.InterestRate
                };

                await _loanRepository.AddNewCustomerAndLoanAsync(newCustomer, newLoan);

                MessageBox.Show("Customer and Loan added successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
            InterestRate = 0;
            DailyPayment = 0;
            TotalCollectible = 0;
            Terms = 0;
            DateLoaned = DateTime.Now;
            DueDate = null;
            SelectedLoanType = string.Empty;
        }
    }
}