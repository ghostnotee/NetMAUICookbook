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
    private readonly SessionService _sessionService = SessionService.Instance;


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

    [RelayCommand]
    private async Task GoogleSignInAsync()
    {
        try
        {
            await _webService.GoogleAuthAsync();
            await Shell.Current.GoToAsync(nameof(UsersPage));
        }
        catch (Exception ex) when (!(ex is TaskCanceledException))
        {
            await Shell.Current.DisplayAlert("Sign in failed", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task SessionLogInAsync()
    {
        if (await _sessionService.UseExistingSession()) await Shell.Current.GoToAsync(nameof(UsersPage));
    }
}