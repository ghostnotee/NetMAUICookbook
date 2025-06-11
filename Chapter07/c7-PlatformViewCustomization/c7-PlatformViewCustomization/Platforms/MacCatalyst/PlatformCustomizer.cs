using UIKit;

namespace c7_PlatformViewCustomization;

public static partial class PlatformCustomizer
{
    public static partial void CustomizeEntry(object platformView)
    {
        if (platformView is UITextField textField)
        {
            // textField.TextColor = UIColor.FromRGB(0, 122, 255);
            // textField.BackgroundColor = UIColor.FromRGB(240, 240, 240);
            textField.TintColor = UIColor.FromRGBA(0, 255, 209, 255);
        }
    }
}