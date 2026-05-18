using LendingServices.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

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