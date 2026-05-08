using System.Windows;
using System.Windows.Controls;
using LendingServices.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LendingServices.Views
{
    public partial class DailyCollection : Page
    {
        private readonly DailyCollectionViewModel _viewModel;

        public DailyCollection()
        {
            InitializeComponent();

            _viewModel = App.ServiceProvider.GetRequiredService<DailyCollectionViewModel>();
            DataContext = _viewModel;

            this.Loaded += DailyCollection_Loaded;
        }

        private async void DailyCollection_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadDataAsync();
        }

        private void PreviewPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintPreview previewPage = new PrintPreview(_viewModel.Customers);
            this.NavigationService?.Navigate(previewPage);
        }
    }
}