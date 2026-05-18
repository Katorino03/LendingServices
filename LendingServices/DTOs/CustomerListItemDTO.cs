using System;

namespace LendingServices.DTOs
{
    public class CustomerListItemDTO
    {
        public int CustomerId { get; set; }
        public string AccountNo { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;

        public decimal LoanAmount { get; set; }
        public decimal Balance { get; set; }
        public decimal DailyPayment { get; set; }
        public DateTime DueDate { get; set; }

        public string Status => (Balance > 0 && DueDate < DateTime.Now) ? "Overdue" : "Active";
        public string StatusColor => Status == "Overdue" ? "#EF4444" : "#22C55E";
        public string LoanTag { get; set; }
    }
}