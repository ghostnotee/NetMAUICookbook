using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace c2_ViewModelCommunication.ViewModels;

public partial class SecurityMonitorViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<string> securityAlerts;

    public SecurityMonitorViewModel()
    {
        securityAlerts = new ObservableCollection<string>();
        WeakReferenceMessenger.Default.Register<AlertMessage, string>(this, AlertTypes.Security, (r, alert) => { SecurityAlerts.Add(alert.Value); });
    }
}

public class RequestAlert : RequestMessage<string>
{
}