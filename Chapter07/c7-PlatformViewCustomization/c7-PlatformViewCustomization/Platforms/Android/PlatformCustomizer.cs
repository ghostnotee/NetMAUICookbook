using AndroidX.AppCompat.Widget;
using Color = Android.Graphics.Color;

namespace c7_PlatformViewCustomization;

public static partial class PlatformCustomizer
{
    public static partial void CustomizeEntry(object platformView)
    {
        AppCompatEditText editor = (AppCompatEditText)platformView;
        editor.SetHighlightColor(Color.Argb(255, 0, 255, 209));
    }
}