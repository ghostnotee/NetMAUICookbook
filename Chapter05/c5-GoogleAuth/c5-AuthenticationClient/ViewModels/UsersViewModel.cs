using System.Collections.ObjectModel;
using c5_AuthenticationClient.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c5_AuthenticationClient.ViewModels;

public partial class UsersViewModel : ObservableObject
{
    private readonly WebService _webService = WebService.Instance;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(DeleteUserCommand))]
    private bool _allowDelete;

    [ObservableProperty] private User? _loggedInUser;

    [ObservableProperty] private ObservableCollection<User> _users = null!;

    [RelayCommand(CanExecute = nameof(CanDeleteUser))]
    async Task DeleteUser(User user)
    {
        try
        {
            await _webService.DeleteUserAsync(user.Email);
            Users.Remove(user);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private bool CanDeleteUser() => AllowDelete;

    [RelayCommand]
    private async Task Initialize()
    {
        Users = new ObservableCollection<User>(await _webService.GetUsersAsync());
        AllowDelete = await _webService.CanDeleteUsersAsync();
        LoggedInUser = await _webService.GetCurrentUserAsync();
    }
}