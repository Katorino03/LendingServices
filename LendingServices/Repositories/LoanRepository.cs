using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LendingServices.Data;
using LendingServices.DTOs;
using LendingServices.Models;
using Microsoft.EntityFrameworkCore;
namespace LendingServices.Repositories

{

    public class LoanRepository : Repository<Loan>, ILoanRepository

    {

        public LoanRepository(AngCoolDbContext context) : base(context)

        {

        }



        public async Task<List<DailyCollectionDTO>> GetDailyCollectionListAsync()

        {

            return await _context.Loans

            .Include(l => l.Customer)

            .Include(l => l.Payments)

            .Where(l => l.IsActive || l.LoanType == "Overdue")

            .Select(l => new DailyCollectionDTO

            {

                CustomerId = l.Customer.Id,

                LoanId = l.Id,

                AccountNo = l.Customer.AccountNo,

                CustomerName = l.Customer.Name,

                DailyPayment = l.DailyPayment,

                Balance = l.TotalAmountToPay - l.Payments.Sum(p => p.AmountPaid),

                AmountPaidToday = 0,

                LoanTag = l.LoanType

            })

            .ToListAsync();

        }



        public async Task SaveDailyCollectionsAsync(IEnumerable<DailyCollectionDTO> collections)

        {

            var today = DateTime.Now.Date;



            foreach (var item in collections)

            {

                var payment = new Payment

                {

                    LoanId = item.LoanId,

                    AmountPaid = item.AmountPaidToday,

                    PaymentDate = today

                };



                await _context.Payments.AddAsync(payment);



                if (item.Balance - item.AmountPaidToday <= 0)

                {

                    var loan = await _context.Loans.FindAsync(item.LoanId);

                    if (loan != null)

                    {

                        loan.IsActive = false;

                        loan.LoanType = "Fully Paid";

                    }

                }

            }



            await _context.SaveChangesAsync();

        }



        public async Task AddNewCustomerAndLoanAsync(Customer customer, Loan loan)

        {

            customer.Loans = new List<Loan> { loan };

            await _context.Customers.AddAsync(customer);

            await _context.SaveChangesAsync();

        }



        public async Task<List<CustomerListItemDTO>> GetCustomerListAsync()

        {

            return await _context.Loans

            .Include(l => l.Customer)

            .Include(l => l.Payments)

            .Where(l => l.IsActive)

            .Select(l => new CustomerListItemDTO

            {

                CustomerId = l.Customer.Id,

                AccountNo = l.Customer.AccountNo,

                Name = l.Customer.Name,

                Address = l.Customer.Address,

                Contact = l.Customer.ContactNo,

                LoanAmount = l.TotalAmountToPay,

                DailyPayment = l.DailyPayment,

                DueDate = l.DueDate,

                Balance = l.TotalAmountToPay - l.Payments.Sum(p => p.AmountPaid),

                LoanTag = l.LoanType

            })

            .ToListAsync();

        }



        public async Task SaveDailySummaryAsync(DateTime date, decimal totalCollection, decimal expenses, decimal additionalRelease, decimal netCollection)

        {

            var existingSummary = await _context.DailySummaries

            .FirstOrDefaultAsync(s => s.SummaryDate.Date == date.Date);



            if (existingSummary != null)

            {

                existingSummary.TotalCollection += totalCollection;

                existingSummary.Expenses += expenses;

                existingSummary.AdditionalRelease += additionalRelease;

                existingSummary.NetCollection += netCollection;

            }

            else

            {

                var newSummary = new DailySummary

                {

                    SummaryDate = date.Date,

                    TotalCollection = totalCollection,

                    Expenses = expenses,

                    AdditionalRelease = additionalRelease,

                    NetCollection = netCollection

                };

                await _context.DailySummaries.AddAsync(newSummary);

            }



            await _context.SaveChangesAsync();

        }

        public async Task<List<CustomerLookupDTO>> GetActiveCustomersLookupAsync()
        {
            return await _context.Loans
                .Include(l => l.Customer)
                .Where(l => l.IsActive)
                .Select(l => new CustomerLookupDTO
                {
                    CustomerId = l.Customer.Id,
                    CustomerName = l.Customer.Name
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<LoanDetailsDTO> GetActiveLoanDetailsAsync(int customerId)
        {
            return await _context.Loans
                .Include(l => l.Customer)
                .Where(l => l.Customer.Id == customerId && l.IsActive)
                .Select(l => new LoanDetailsDTO
                {
                    LoanId = l.Id,
                    CustomerName = l.Customer.Name,
                    AccountNo = l.Customer.AccountNo,
                    Address = l.Customer.Address,
                    ContactNo = l.Customer.ContactNo,
                    DateLoaned = l.DateLoaned, // If your Loan class uses a different name like ReleaseDate, change it here
                    DueDate = l.DueDate,
                    DailyPayment = l.DailyPayment,
                    TotalCollectible = l.TotalAmountToPay
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<LedgerItemDTO>> GetLoanLedgerAsync(int loanId)
        {
            // Fetch payment history records for this specific loan from database
            var payments = await _context.Payments
                .Where(p => p.LoanId == loanId)
                .OrderBy(p => p.PaymentDate)
                .ToListAsync();

            // Transform payment items sequentially into ledger rows for the UI Grid
            var ledgerList = new List<LedgerItemDTO>();
            for (int i = 0; i < payments.Count; i++)
            {
                ledgerList.Add(new LedgerItemDTO
                {
                    Day = i + 1,
                    Date = payments[i].PaymentDate,
                    Principal = payments[i].AmountPaid,
                    IsSigned = true // Mark existing payments as signed
                });
            }

            return ledgerList;
        }
    }
}

    



