using System.Diagnostics;
using Microsoft.Maui.Handlers;

namespace c8_DebugVsRelease;

public class LoadingTimePage : ContentPage
{
    private static readonly Stopwatch LoadingStopwatch = new();
    public static void StartTimer() => LoadingStopwatch.Restart();

    public static void ShowTimeElapsed()
    {
        LoadingStopwatch.Stop();
        Shell.Current.DisplayAlert("Elapsed miliseconds", LoadingStopwatch.ElapsedMilliseconds.ToString(), "OK");
    }
}

public partial class LoadingTimePageHandler : PageHandler
{
}