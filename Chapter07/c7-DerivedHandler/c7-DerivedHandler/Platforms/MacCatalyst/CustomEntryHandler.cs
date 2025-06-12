using Microsoft.Maui.Platform;
using UIKit;

namespace c7_DerivedHandler;

public partial class CustomEntryHandler
{
    static partial void MapSelectionColor(CustomEntryHandler handler, CustomEntry entry)
    {
        if (handler.PlatformView is UITextField textField) textField.TintColor = entry.SelectionColor.ToPlatform();
    }
}