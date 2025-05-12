using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c3_LottieAnimations;

public partial class MyViewModel : ObservableObject
{
    [ObservableProperty] private string _statusMessage = "Let's run! ghostNoté;";

    [RelayCommand]
    private async Task MindLoadingAsync()
    {
        await Task.Delay(5000);
        StatusMessage = "Complete! Let's run again!";
    }
}