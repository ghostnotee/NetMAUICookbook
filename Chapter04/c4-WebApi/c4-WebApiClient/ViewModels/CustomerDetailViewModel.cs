using c4_WebApi.DataAccess;
using c4_WebApi.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c4_WebApi.ViewModels;

public partial class CustomerDetailViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private Customer _item = null!;
    protected Func<Customer, Task> ParentRefreshAction { get; set; } = null!;

    public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Item", out var currentItem)) Item = (currentItem as Customer)!;
        if (query.TryGetValue("ParentRefreshAction", out var parentRefreshAction)) ParentRefreshAction = (parentRefreshAction as Func<Customer, Task>)!;
        query.Clear();
    }

    [RelayCommand]
    private async Task ShowEditFormAsync()
    {
        using var uof = new CrmUnitOfWork();
        var editedItem = await uof.Items.GetByIdAsync(Item!.Id);
        await Shell.Current.GoToAsync(nameof(CustomerEditPage),
            new Dictionary<string, object>
            {
                { "ParentRefreshAction", (Func<Customer, Task>)ItemEditedAsync },
                { "Item", editedItem }
            });
    }

    private async Task ItemEditedAsync(Customer customer)
    {
        Item = customer;
        await ParentRefreshAction(customer);
    }
}