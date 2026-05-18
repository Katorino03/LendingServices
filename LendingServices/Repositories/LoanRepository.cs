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
                .Where(l => l.IsActive)
                .Select(l => new DailyCollectionDTO
                {
                    CustomerId = l.Customer.Id,
                    LoanId = l.Id,
                    AccountNo = l.Customer.AccountNo,
                    CustomerName = l.Customer.Name,
                    DailyPayment = l.DailyPayment,
                    Balance = l.TotalAmountToPay - l.Payments.Sum(p => p.AmountPaid),
                    AmountPaidToday = 0
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
    }
}