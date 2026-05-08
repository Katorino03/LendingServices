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
    }
}