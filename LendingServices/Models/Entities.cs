using System;
using System.Collections.Generic;

namespace LendingServices.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string AccountNo { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }

    public class Loan
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public DateTime DateLoaned { get; set; }
        public DateTime DueDate { get; set; }

        public decimal PrincipalAmount { get; set; }
        public decimal TotalAmountToPay { get; set; }
        public decimal DailyPayment { get; set; }
        public int TermsInDays { get; set; }

        public bool IsActive { get; set; } = true;
        public string? LoanType { get; set; }

        public Customer Customer { get; set; } = null!;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

    public class Payment
    {
        public int Id { get; set; }
        public int LoanId { get; set; }

        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }

        public string CollectedBy { get; set; } = string.Empty;

        public Loan Loan { get; set; } = null!;
    }

    public class DailyExpense
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}