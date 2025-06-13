using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c8_AsyncLoading;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private string description;

    [ObservableProperty] private string productName;

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        await Task.Delay(2000);
        ProductName = "Some product";
        Description = "Product description";
    }
}