using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c8_AsyncLoading;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private string? _description;

    [ObservableProperty] private string? _productName;

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        await Task.Delay(2000);
        ProductName = "Some product";
        Description = "Product description";
    }
}