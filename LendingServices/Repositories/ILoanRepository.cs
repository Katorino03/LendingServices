using System.Collections.Generic;
using System.Threading.Tasks;
using LendingServices.DTOs;
using LendingServices.Models;

namespace LendingServices.Repositories
{
    public interface ILoanRepository : IRepository<Loan>
    {
        Task<List<DailyCollectionDTO>> GetDailyCollectionListAsync();
        Task SaveDailyCollectionsAsync(IEnumerable<DailyCollectionDTO> collections);
        Task AddNewCustomerAndLoanAsync(Customer customer, Loan loan);
        Task<List<CustomerListItemDTO>> GetCustomerListAsync();
        Task SaveDailySummaryAsync(DateTime date, decimal totalCollection, decimal expenses, decimal additionalRelease, decimal netCollection);
        Task<List<CustomerLookupDTO>> GetActiveCustomersLookupAsync();
        Task<LoanDetailsDTO> GetActiveLoanDetailsAsync(int customerId);
        Task<List<LedgerItemDTO>> GetLoanLedgerAsync(int loanId);

    }
}