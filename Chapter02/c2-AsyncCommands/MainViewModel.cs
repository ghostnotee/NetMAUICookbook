using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c2_AsyncCommands;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UpdateTextCommand))]
    int count;

    [ObservableProperty] string textValue = "Click Me!";

    [RelayCommand(IncludeCancelCommand = true)]
    public async Task UpdateTextAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(5000, token);
        }
        catch (OperationCanceledException)
        {
            return;
        }
        
        Count++;
        TextValue = Count == 1 ? $"Clicked {Count} time" : $"Clicked {Count} times";
    }
    
    // public void Cancel() {
    //     UpdateTextCommand.Cancel();
    // }
}