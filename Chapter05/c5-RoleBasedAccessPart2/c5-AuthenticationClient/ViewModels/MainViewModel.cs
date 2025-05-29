using c5_AuthenticationClient.Model;
using c5_AuthenticationClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace c5_AuthenticationClient.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly WebService _webService = WebService.Instance;
    [ObservableProperty] private string _email = null!;
    [ObservableProperty] private string _password = null!;

    [RelayCommand]
    private async Task LogInAsync()
    {
        try
        {
            var tokenInfo = await _webService.Authenticate(Email, Password);
            await Shell.Current.GoToAsync(nameof(UsersPage));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}