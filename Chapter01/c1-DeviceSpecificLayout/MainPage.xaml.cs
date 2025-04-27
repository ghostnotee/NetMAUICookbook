namespace c1_DeviceSpecificLayout;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        if (DeviceInfo.Current.Idiom == DeviceIdiom.Desktop)
        {
            grid1.Height = CalculateDesktopHeight();
        }
        else if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
        {
            grid1.Height = CalculateMobileHeight();
        }
    }
}