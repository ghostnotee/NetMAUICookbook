using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c2_TroubleshootBindings;

public partial class MyViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<Customer> _customers;

    public MyViewModel()
    {
        Customers = new ObservableCollection<Customer>();
        for (var i = 1; i < 30; i++) Customers.Add(new Customer { Id = i, Name = "Name" + i });
    }

    [RelayCommand]
    private void DeleteCustomer(Customer customer)
    {
        Customers.Remove(customer);
    }
}

public class Customer
{
    public int Id { get; set; }

    public string? Name { get; set; }
}