using c4_CreateDelete.DataAccess;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c4_CreateDelete.ViewModels;

public partial class CustomerEditViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private Customer? _item;
    protected Func<Customer, Task>? ParentRefreshAction { get; set; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Item", out var currentItem)) Item = (Customer)currentItem;

        if (query.TryGetValue("ParentRefreshAction", out var parentRefreshAction)) ParentRefreshAction = (Func<Customer, Task>)parentRefreshAction;

        query.Clear();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        await using var context = new CrmContext();
        context.Customers.Add(Item);
        context.SaveChanges();
        await ParentRefreshAction(Item);
        await Shell.Current.GoToAsync("..");
    }
}