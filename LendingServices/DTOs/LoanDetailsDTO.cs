using System;

namespace LendingServices.DTOs
{
    public class LoanDetailsDTO
    {
        public int LoanId { get; set; }
        public string CustomerName { get; set; }
        public string AccountNo { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public DateTime DateLoaned { get; set; }
        public DateTime DueDate { get; set; }
        public decimal DailyPayment { get; set; }
        public decimal TotalCollectible { get; set; }
    }
}