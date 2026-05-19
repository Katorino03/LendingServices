using System;

namespace LendingServices.Models
{
    public class DailySummary
    {
        public int Id { get; set; }
        public DateTime SummaryDate { get; set; }
        public decimal TotalCollection { get; set; }
        public decimal Expenses { get; set; }
        public decimal AdditionalRelease { get; set; }
        public decimal NetCollection { get; set; }
    }
}