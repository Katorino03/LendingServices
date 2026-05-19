using Microsoft.Extensions.DependencyInjection;
using LendingServices.ViewModels;
using System.Windows.Controls;

namespace LendingServices.Views
{
    public partial class Dashboard : Page
    {
        private readonly DashboardViewModel _viewModel;

        public Dashboard()
        {
            InitializeComponent();

            _viewModel = App.ServiceProvider.GetRequiredService<DashboardViewModel>();
            DataContext = _viewModel;

            this.Loaded += async (s, e) => await _viewModel.LoadDataAsync();
        }
    }
}
