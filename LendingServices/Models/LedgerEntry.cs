namespace LendingServices.Models
{
    public class LedgerEntry
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public string Principal { get; set; }
        public string Signature { get; set; }
    }
}
