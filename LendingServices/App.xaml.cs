using LendingServices.Data;
using LendingServices.Models;
using LendingServices.Repositories;
using LendingServices.ViewModels;
using LendingServices.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace LendingServices
{
    public partial class App : Application
    {
        private readonly IHost _host;
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<AngCoolDbContext>();
                    services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
                    services.AddTransient<ILoanRepository, LoanRepository>();
                    services.AddTransient<DailyCollectionViewModel>();
                    services.AddTransient<AddCustomerViewModel>();
                    services.AddTransient<CustomerListViewModel>();
                    services.AddTransient<DashboardViewModel>();
                    services.AddTransient<BorrowerCopyViewModel>();
                    services.AddTransient<LoginWindow>();
                    services.AddTransient<CustomerList>();
                    services.AddScoped<IUserRepository, UserRepository>();
                })
                .Build();

            ServiceProvider = _host.Services;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            using (var scope = _host.Services.CreateScope())
            {
                // This after debug but still error

                var dbContext = scope.ServiceProvider.GetRequiredService<AngCoolDbContext>();

                try
                {
                    await dbContext.Database.MigrateAsync();
                }
                catch (Microsoft.Data.Sqlite.SqliteException ex) when (ex.Message.Contains("already exists"))
                {
                    // The database tables already exist but the EF Migrations history is out of sync.
                    System.Diagnostics.Debug.WriteLine($"Migration skipped: {ex.Message}");
                }

                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                await EnsureDefaultAdminExists(userRepository);

                // Original code without try-catch
                //var dbContext = scope.ServiceProvider.GetRequiredService<AngCoolDbContext>();
                //await dbContext.Database.MigrateAsync();

                //var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                //await EnsureDefaultAdminExists(userRepository);
            }

            var loginWindow = _host.Services.GetRequiredService<LoginWindow>();
            loginWindow.Show();

            base.OnStartup(e);
        }

        private async Task EnsureDefaultAdminExists(IUserRepository userRepository)
        {
            if (!await userRepository.HasAnyUsersAsync())
            {
                var defaultAdmin = new UserAccount
                {
                    FullName = "Admin User",
                    Username = "admin",
                    Password = "password123",
                    SecurityQuestion = "What is the name of this app?",
                    SecurityAnswer = "Ang Cool"
                };

                await userRepository.AddUserAsync(defaultAdmin);
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}