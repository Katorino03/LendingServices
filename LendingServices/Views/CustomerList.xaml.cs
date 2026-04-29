using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LendingServices.Models;  

namespace LendingServices.Views
{
    public partial class CustomerList : Page
    {
        public CustomerList()
        {
            InitializeComponent();

            var data = new List<Customer>
            {
                new Customer { AccountNo="407", Name="JELBERT ANTONIO", Address="San Isidro, Leyte", Contact="09123456789", Loan="₱2,026", Balance="₱1,200", Daily="₱100", DueDate="5/15/2026", Status="Active", StatusColor="#22C55E"},
                new Customer { AccountNo="408", Name="MARIA CLARA", Address="Tacloban City", Contact="09234567890", Loan="₱3,000", Balance="₱2,400", Daily="₱120", DueDate="5/30/2026", Status="Active", StatusColor="#22C55E"},
                new Customer { AccountNo="409", Name="PEDRO SANTOS", Address="Palo, Leyte", Contact="09345678901", Loan="₱5,000", Balance="₱650", Daily="₱130", DueDate="4/25/2026", Status="Overdue", StatusColor="#EF4444"}
            };

            myDataGrid.ItemsSource = data;
        }

        private void ViewLedger_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var customer = button?.DataContext as Customer;

            if (customer != null)
            {
                var ledgerEntries = new ObservableCollection<LedgerEntry>();
                for (int day = 1; day <= 10; day++)
                {
                    ledgerEntries.Add(new LedgerEntry
                    {
                        Day = day,
                        Date = DateTime.Now.AddDays(day),
                        Principal = customer.Daily,
                        Signature = ""
                    });
                }

                BorrowerCopy borrowerPage = new BorrowerCopy(customer, ledgerEntries);
                this.NavigationService?.Navigate(borrowerPage);
            }
        }

        public class Customer
        {
            public string AccountNo { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Contact { get; set; }
            public string Loan { get; set; }
            public string Balance { get; set; }
            public string Daily { get; set; }
            public string DueDate { get; set; }
            public string Status { get; set; }
            public string StatusColor { get; set; }
        }
    }
}
