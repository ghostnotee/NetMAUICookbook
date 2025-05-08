using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace c2_ViewModelCommunication.ViewModels;

public partial class PerformanceMonitorViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<string> _performanceAlerts;

    public PerformanceMonitorViewModel()
    {
        _performanceAlerts = new ObservableCollection<string>();
        WeakReferenceMessenger.Default.Register<AlertMessage, string>(this, AlertTypes.Performance, (r, alert) => { PerformanceAlerts.Add(alert.Value); });
    }
}