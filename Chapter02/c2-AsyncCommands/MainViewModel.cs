using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c2_AsyncCommands;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UpdateTextCommand))]
    int count;

    [ObservableProperty] string textValue = "Click Me!";

    [RelayCommand(CanExecute = nameof(CanUpdateText))]
    public void UpdateText()
    {
        Count++;
        TextValue = Count == 1 ? $"Clicked {Count} time" : $"Clicked {Count} times";
    }

    public bool CanUpdateText()
    {
        return Count < 3;
    }
}