using System.Collections.ObjectModel;
using c4_LocalDatabaseConnection.DataAccess;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c4_LocalDatabaseConnection.ViewModels;

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
}