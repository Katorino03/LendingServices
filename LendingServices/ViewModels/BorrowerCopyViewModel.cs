using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Linq;
using LendingServices.DTOs; 
using LendingServices.Repositories;

namespace LendingServices.ViewModels
{
    public class BorrowerCopyViewModel : INotifyPropertyChanged
    {
        private readonly ILoanRepository _loanRepository;

        public ObservableCollection<CustomerLookupDTO> CustomerList { get; set; } = new();
        public ObservableCollection<LedgerItemDTO> LedgerItems { get; set; } = new();

        public BorrowerCopyViewModel(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        private CustomerLookupDTO _selectedCustomer;
        public CustomerLookupDTO SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();

                if (_selectedCustomer != null)
                {
                    _ = FetchCustomerLedgerAsync(_selectedCustomer.CustomerId);
                }
            }
        }


        private string _name;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }

        private string _accountNo;
        public string AccountNo { get => _accountNo; set { _accountNo = value; OnPropertyChanged(); } }

        private string _address;
        public string Address { get => _address; set { _address = value; OnPropertyChanged(); } }

        private string _contact;
        public string Contact { get => _contact; set { _contact = value; OnPropertyChanged(); } }

        private DateTime? _dateLoaned;
        public DateTime? DateLoaned { get => _dateLoaned; set { _dateLoaned = value; OnPropertyChanged(); } }

        private DateTime? _dueDate;
        public DateTime? DueDate { get => _dueDate; set { _dueDate = value; OnPropertyChanged(); } }

        private decimal _dailyPayment;
        public decimal DailyPayment { get => _dailyPayment; set { _dailyPayment = value; OnPropertyChanged(); } }

        private decimal _loanAmount;
        public decimal LoanAmount { get => _loanAmount; set { _loanAmount = value; OnPropertyChanged(); } }

        private decimal _totalPaid;
        public decimal TotalPaid { get => _totalPaid; set { _totalPaid = value; OnPropertyChanged(); } }

        private decimal _balance;
        public decimal Balance { get => _balance; set { _balance = value; OnPropertyChanged(); } }



        public async Task LoadCustomerListAsync()
        {
            var customers = await _loanRepository.GetActiveCustomersLookupAsync();
            CustomerList.Clear();
            foreach (var customer in customers)
            {
                CustomerList.Add(customer);
            }
        }

        private async Task FetchCustomerLedgerAsync(int customerId)
        {
            var loanData = await _loanRepository.GetActiveLoanDetailsAsync(customerId);

            if (loanData != null)
            {
                Name = loanData.CustomerName;
                AccountNo = loanData.AccountNo;
                Address = loanData.Address;
                Contact = loanData.ContactNo;
                DateLoaned = loanData.DateLoaned;
                DueDate = loanData.DueDate;
                DailyPayment = loanData.DailyPayment;
                LoanAmount = loanData.TotalCollectible;

                var history = await _loanRepository.GetLoanLedgerAsync(loanData.LoanId);
                LedgerItems.Clear();

                decimal totalPaid = 0;

                foreach (var item in history)
                {
                    LedgerItems.Add(item);
                    totalPaid += item.Principal;
                }

                TotalPaid = totalPaid;
                Balance = LoanAmount - TotalPaid;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}