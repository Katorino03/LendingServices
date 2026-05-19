using LendingServices.Models;
using LendingServices.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LendingServices.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AngCoolDbContext _context;

        public UserRepository(AngCoolDbContext context)
        {
            _context = context;
        }

        public async Task<UserAccount?> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;

            return await _context.UserAccounts
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);
            return user != null && user.Password == password;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.UserAccounts.AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task CreateUserAsync(string username, string fullName, string password, string securityQuestion, string securityAnswer)
        {
            var newUser = new UserAccount
            {
                Username = username,
                FullName = fullName,
                Password = password,
                SecurityQuestion = securityQuestion,
                SecurityAnswer = securityAnswer,
            };

            _context.UserAccounts.Add(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(string originalUsername, string newUsername, string fullName, string password, string securityQuestion, string securityAnswer)
        {
            var existingUser = await GetUserByUsernameAsync(originalUsername);
            if (existingUser != null)
            {
                existingUser.Username = newUsername;
                existingUser.FullName = fullName;
                existingUser.Password = password;
                existingUser.SecurityQuestion = securityQuestion;
                existingUser.SecurityAnswer = securityAnswer;

                _context.UserAccounts.Update(existingUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserAsync(UserAccount user)
        {
            _context.UserAccounts.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserAsync(UserAccount user)
        {
            _context.UserAccounts.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasAnyUsersAsync()
        {
            return await _context.UserAccounts.AnyAsync();
        }
    }
}