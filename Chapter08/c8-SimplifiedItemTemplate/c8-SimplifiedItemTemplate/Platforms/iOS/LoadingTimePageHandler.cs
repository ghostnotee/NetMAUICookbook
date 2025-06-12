using Microsoft.Maui.Platform;
using ContentView = Microsoft.Maui.Platform.ContentView;

namespace c8_DebugVsRelease;

public partial class LoadingTimePageHandler
{
    protected override ContentView CreatePlatformView()
    {
        if (ViewController == null)
            ViewController = new CustomViewController(VirtualView, MauiContext!);
        if (ViewController is PageViewController pc && pc.CurrentPlatformView is ContentView pv)
            return pv;
        if (ViewController.View is ContentView cv)
            return cv;
        throw new InvalidOperationException($"PageViewController.View must be a {nameof(ContentView)}");
    }

    private class CustomViewController(IView page, IMauiContext mauiContext) : PageViewController(page, mauiContext)
    {
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            LoadingTimePage.ShowTimeElapsed();
        }
    }
}