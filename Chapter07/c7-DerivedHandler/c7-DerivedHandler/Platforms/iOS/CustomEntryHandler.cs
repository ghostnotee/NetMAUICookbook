using Microsoft.Maui.Platform;

namespace c7_DerivedHandler;

public partial class CustomEntryHandler
{
    static partial void MapSelectionColor(CustomEntryHandler handler, CustomEntry entry)
    {
        if (handler.PlatformView != null) handler.PlatformView.TintColor = entry.SelectionColor.ToPlatform();
    }
}