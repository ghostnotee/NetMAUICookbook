using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c2_GeneratedViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UpdateTextCommand))]
    int count;

    [ObservableProperty] string textValue = "Click Me!";

    [RelayCommand(CanExecute = nameof(CanUpdateText))]
    public void UpdateText()
    {
        Count++;
        if (Count == 1)
            TextValue = $"Clicked {Count} time";
        else
            TextValue = $"Clicked {Count} times";
    }

    public bool CanUpdateText()
    {
        return Count < 3;
    }
}