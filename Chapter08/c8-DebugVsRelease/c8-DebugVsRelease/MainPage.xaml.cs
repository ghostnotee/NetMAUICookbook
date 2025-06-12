namespace c8_DebugVsRelease;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnOpenTestPageClicked(object sender, EventArgs e)
    {
        LoadingTimePage.StartTimer();
        await Shell.Current.GoToAsync(nameof(TestPage), false);
    }
}