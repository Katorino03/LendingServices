using System;
using LendingServices.Data;
using LendingServices.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using LendingServices.ViewModels;

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
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped<ILoanRepository, LoanRepository>();
                    services.AddTransient<DailyCollectionViewModel>();
                    services.AddTransient<MainWindow>();
                })
                .Build();

            ServiceProvider = _host.Services;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            using (var scope = _host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AngCoolDbContext>();
                await dbContext.Database.MigrateAsync();
            }

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

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