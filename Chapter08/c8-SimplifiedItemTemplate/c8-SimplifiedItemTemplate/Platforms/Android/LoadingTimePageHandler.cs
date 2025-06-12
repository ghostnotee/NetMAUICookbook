using Android.Views;
using Microsoft.Maui.Platform;
using Object = Java.Lang.Object;

namespace c8_DebugVsRelease;

public partial class LoadingTimePageHandler
{
    protected override ContentViewGroup CreatePlatformView()
    {
        ContentViewGroup platformView = base.CreatePlatformView();
        platformView.ViewTreeObserver!.AddOnGlobalLayoutListener(new MyGlobalLayoutListener(platformView));
        return platformView;
    }
}

internal class MyGlobalLayoutListener(ContentViewGroup platformView) : Object, ViewTreeObserver.IOnGlobalLayoutListener
{
    public void OnGlobalLayout()
    {
        LoadingTimePage.ShowTimeElapsed();
        platformView.ViewTreeObserver!.RemoveOnGlobalLayoutListener(this);
    }
}