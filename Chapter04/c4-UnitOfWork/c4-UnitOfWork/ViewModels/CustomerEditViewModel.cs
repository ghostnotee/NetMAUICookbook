using c4_UnitOfWork.DataAccess;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c4_UnitOfWork.ViewModels;

public partial class CustomerEditViewModel : CustomerDetailViewModel
{
    [ObservableProperty] private bool _isNewItem;

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("IsNewItem", out var isNew)) IsNewItem = (bool)isNew;
        base.ApplyQueryAttributes(query);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        using var uof = new CrmUnitOfWork();
        if (IsNewItem)
            await uof.Items.AddAsync(Item);
        else
            await uof.Items.UpdateAsync(Item);
        await uof.SaveAsync();
        await ParentRefreshAction(Item);
        await Shell.Current.GoToAsync("..");
    }
}