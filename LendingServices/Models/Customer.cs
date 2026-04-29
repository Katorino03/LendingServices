namespace LendingServices.Models
{
    public class Customer
    {
        public int AccountNo { get; set; }
        public string Name { get; set; }
        public decimal DailyPayment { get; set; }
        public decimal Balance { get; set; }
        public decimal AmountPaidToday { get; set; }
    }
}
