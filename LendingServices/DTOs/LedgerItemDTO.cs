using System;

namespace LendingServices.DTOs
{
    public class LedgerItemDTO
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public decimal Principal { get; set; }
        public bool IsSigned { get; set; }
    }
}