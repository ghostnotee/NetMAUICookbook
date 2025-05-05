using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace c2_UiAndViewModelInteraction;

public partial class MyViewModel : ObservableObject
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddCustomerCommand))]
    private ObservableCollection<Customer>? _customers;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(InitializeCommand))]
    private bool _isInitialized;

    [RelayCommand(CanExecute = nameof(CanInitialize))]
    private async Task InitializeAsync()
    {
        Customers = new ObservableCollection<Customer>(await DummyService.GetCustomersAsync());
        IsInitialized = true;
    }

    [RelayCommand(CanExecute = nameof(CanAddCustomer))]
    private void AddCustomer()
    {
        if (Customers != null)
        {
            Customers.Add(new Customer { Id = Customers.Count, Name = "New Customer" });
            WeakReferenceMessenger.Default.Send(Customers.Last());
        }
    }

    private bool CanAddCustomer()
    {
        return Customers != null;
    }

    private bool CanInitialize() => !IsInitialized;
}

public class Customer
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public static class DummyService
{
    public static async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        await Task.Delay(5000);
        return new List<Customer>
        {
            new() { Id = 1, Name = "Jim" },
            new() { Id = 2, Name = "Bob" }
        };
    }
}