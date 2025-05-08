using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace c2_ViewModelCommunication.ViewModels;

public partial class AlertGeneratorViewModel : ObservableObject
{
    private int _alertCount;

    [ObservableProperty] private string? _alertText;

    [RelayCommand]
    private void GenerateAlert()
    {
        var channelType = ++_alertCount % 2 == 0 ? AlertTypes.Security : AlertTypes.Performance;
        WeakReferenceMessenger.Default.Send(new AlertMessage(AlertText), channelType);
    }
}

public class AlertMessage(string? value) : ValueChangedMessage<string?>(value)
{
}

public static class AlertTypes
{
    public const string Security = "SecurityAlert";
    public const string Performance = "Performance";
}