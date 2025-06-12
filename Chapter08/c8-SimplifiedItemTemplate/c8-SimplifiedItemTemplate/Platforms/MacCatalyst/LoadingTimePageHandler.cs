using Microsoft.Maui.Platform;
using ContentView = Microsoft.Maui.Platform.ContentView;

namespace c8_DebugVsRelease;

public partial class LoadingTimePageHandler
{
    // CreatePlatformView metodu, iOS versiyonuyla aynı şekilde çalışır.
    // Varsayılan sayfa denetleyicisini CustomViewController ile değiştirir.
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

    // iOS versiyonunda olduğu gibi, sayfa görüntülendiğinde tetiklenecek özel bir UIViewController türevi.
    private class CustomViewController(IView page, IMauiContext mauiContext) : PageViewController(page, mauiContext)
    {
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            // Sayfanın tam olarak görüntülendiği anı işaretler.
            LoadingTimePage.ShowTimeElapsed();
        }
    }
}