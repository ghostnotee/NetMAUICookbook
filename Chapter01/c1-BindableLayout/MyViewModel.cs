using System.Collections.ObjectModel;

namespace c1_BindableLayout;

public class MyViewModel
{
    public MyViewModel()
    {
        DynamicActions = new ObservableCollection<ActionInfo>
        {
            new() { Caption = "Action1" },
            new() { Caption = "Action2" },
            new() { Caption = "Action3" }
        };
    }

    public ObservableCollection<ActionInfo> DynamicActions { get; set; }
}

public class ActionInfo
{
    public string? Caption { get; set; }
}