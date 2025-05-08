using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c2_MvvmDependencyInjection;

public partial class MyViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<Customer>? _customers;

    private readonly IDummyService _dataService;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(InitializeCommand))]
    private bool isInitialized;

    public MyViewModel(IDummyService dataService)
    {
        _dataService = dataService;
    }

    [RelayCommand(CanExecute = nameof(CanInitialize))]
    private async Task InitializeAsync()
    {
        Customers = new ObservableCollection<Customer>(await _dataService.GetCustomersAsync());
        IsInitialized = true;
    }

    private bool CanInitialize() => !IsInitialized;
}

public class Customer
{
    public int ID { get; set; }

    public string? Name { get; set; }
}

public interface IDummyService
{
    Task<IEnumerable<Customer>> GetCustomersAsync();
}

public class DummyService : IDummyService
{
    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        await Task.Delay(5000);
        return new List<Customer>
        {
            new() { ID = 1, Name = "Jim" },
            new() { ID = 2, Name = "Bob" }
        };
    }
}