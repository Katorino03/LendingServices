using CommunityToolkit.Mvvm.ComponentModel;

namespace LendingServices.DTOs
{
    public partial class DailyCollectionDTO : ObservableObject
    {
        public int CustomerId { get; set; }
        public int LoanId { get; set; }
        public string AccountNo { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal DailyPayment { get; set; }
        public decimal Balance { get; set; }

        [ObservableProperty]
        private decimal _amountPaidToday;

        public bool IsFullyPaidToday => AmountPaidToday >= DailyPayment;
    }
}