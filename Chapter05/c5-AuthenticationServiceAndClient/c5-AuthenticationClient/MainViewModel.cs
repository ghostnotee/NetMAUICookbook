using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace c5_AuthenticationClient;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private string _email = null!;

    [ObservableProperty] private string _password = null!;

    [RelayCommand]
    private async Task LogInAsync()
    {
    }
}