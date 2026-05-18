using System.Windows.Controls;
using LendingServices.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LendingServices.Views
{
    public partial class AddCustomer : Page
    {
        public AddCustomer()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<AddCustomerViewModel>();
        }
    }
}