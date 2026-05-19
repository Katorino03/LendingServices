using LendingServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace LendingServices.Data
{
    public class AngCoolDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<DailyExpense> DailyExpenses { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<DailySummary> DailySummaries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "angcool_lending.db" };
                var connectionString = connectionStringBuilder.ToString();
                var connection = new SqliteConnection(connectionString);

                optionsBuilder.UseSqlite(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}