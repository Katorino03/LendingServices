using System;
using LendingServices.Data;
using LendingServices.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using LendingServices.ViewModels;
using LendingServices.Views;

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
                    services.AddTransient<LoginWindow>();
                    services.AddTransient<CustomerList>();
                })
                .Build();

            ServiceProvider = _host.Services;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            using (var scope = _host.Services.CreateScope())
            {
                // var dbContext = scope.ServiceProvider.GetRequiredService<AngCoolDbContext>();
                // await dbContext.Database.MigrateAsync();
            }

            var loginWindow = _host.Services.GetRequiredService<LoginWindow>();
            loginWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}