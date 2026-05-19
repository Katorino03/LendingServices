using LendingServices.Models;
using System.Threading.Tasks;

namespace LendingServices.Repositories
{
    public interface IUserRepository
    {
        Task<UserAccount?> GetUserByUsernameAsync(string username);
        Task<bool> ValidateUserAsync(string username, string password);
        Task AddUserAsync(UserAccount user);
        Task<bool> HasAnyUsersAsync();
        Task UpdateUserAsync(UserAccount user);
        Task<bool> UserExistsAsync(string username);
        Task CreateUserAsync(string username, string fullName, string password, string securityQuestion, string securityAnswer);
        Task UpdateUserAsync(string originalUsername, string newUsername, string fullName, string password, string securityQuestion, string securityAnswer);
    }
}