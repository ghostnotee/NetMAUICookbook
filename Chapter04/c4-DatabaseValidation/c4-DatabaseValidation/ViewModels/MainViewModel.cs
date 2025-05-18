using System.Collections.ObjectModel;
using c4_DatabaseValidation.DataAccess;
using c4_DatabaseValidation.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c4_DatabaseValidation.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<Customer>? _customers;

    [ObservableProperty] private bool _refreshing;

    [RelayCommand]
    private async Task LoadCustomersAsync()
    {
        using var uniOfWork = new CrmUnitOfWork();
        Customers = new ObservableCollection<Customer>(await uniOfWork.Items.GetAllAsync());
        Refreshing = false;
    }

    [RelayCommand]
    private void Showing()
    {
        Refreshing = true;
    }

    [RelayCommand]
    private async Task DeleteCustomerAsync(Customer customer)
    {
        using var uniOfWork = new CrmUnitOfWork();
        await uniOfWork.Items.DeleteAsync(customer);
        try
        {
            await uniOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            return;
        }

        Customers?.Remove(customer);
    }

    [RelayCommand]
    private async Task ShowNewFormAsync()
    {
        await Shell.Current.GoToAsync(nameof(CustomerEditPage),
            new Dictionary<string, object>
            {
                { "ParentRefreshAction", (Func<Customer, Task>)RefreshAddedAsync },
                { "Item", new Customer() },
                { "IsNewItem", true }
            });
    }

    private Task RefreshAddedAsync(Customer addedCustomer)
    {
        Customers!.Add(addedCustomer);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task ShowDetailFormAsync(Customer customer)
    {
        await Shell.Current.GoToAsync(nameof(CustomerDetailPage),
            new Dictionary<string, object>
            {
                { "Item", customer },
                { "ParentRefreshAction", (Func<Customer, Task>)RefreshEditedAsync }
            });
    }

    private async Task RefreshEditedAsync(Customer updatedCustomer)
    {
        var editedItemIndex = -1;
        await Task.Run(() =>
        {
            editedItemIndex = Customers!.Select((customer, index) =>
                new { customer, index }).First(item => item.customer.Id == updatedCustomer.Id).index;
        });
        if (editedItemIndex == -1)
            return;
        Customers![editedItemIndex] = updatedCustomer;
    }
}