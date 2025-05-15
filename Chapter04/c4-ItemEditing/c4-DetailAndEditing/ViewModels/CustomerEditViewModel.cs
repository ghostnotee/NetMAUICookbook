using c4_DetailAndEditing.DataAccess;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c4_DetailAndEditing.ViewModels;

public partial class CustomerEditViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private Customer? _item;
    private Func<Customer, Task> ParentRefreshAction { get; set; } = null!;

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
        context.Customers.Add(Item!);
        await context.SaveChangesAsync();
        await ParentRefreshAction(Item!);
        await Shell.Current.GoToAsync("..");
    }
}