using System.Collections.ObjectModel;
using c4_CreateDelete.DataAccess;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c4_CreateDelete.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<Customer>? _customers;

    [ObservableProperty] private bool _refreshing;

    [RelayCommand]
    private async Task LoadCustomersAsync()
    {
        await Task.Run(() =>
        {
            using var context = new CrmContext();
            Customers = new ObservableCollection<Customer>(context.Customers);
        });
        Refreshing = false;
    }

    [RelayCommand]
    private void Showing()
    {
        Refreshing = true;
    }

    [RelayCommand]
    private void DeleteCustomer(Customer customer)
    {
        var context = new CrmContext();
        context.Customers.Remove(customer);
        context.SaveChanges();
        Customers.Remove(customer);
    }

    [RelayCommand]
    private async Task ShowNewFormAsync()
    {
        await Shell.Current.GoToAsync(nameof(CustomerEditPage),
            parameters: new Dictionary<string, object>
            {
                { "ParentRefreshAction", (Func<Customer, Task>)RefreshAddedAsync },
                { "Item", new Customer() },
            });
    }

    private Task RefreshAddedAsync(Customer addedCustomer)
    {
        Customers!.Add(addedCustomer);
        return Task.CompletedTask;
    }
}