using CommunityToolkit.Mvvm.ComponentModel;

namespace c3_DarkAndLightThemes;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
}

public class ThemeInfo
{
    public static ThemeInfo System = new(AppTheme.Unspecified, "System");
    public static ThemeInfo Light = new(AppTheme.Light, "Light");
    public static ThemeInfo Dark = new(AppTheme.Dark, "Dark");

    public ThemeInfo(AppTheme theme, string caption)
    {
        AppTheme = theme;
        Caption = caption;
    }

    public AppTheme AppTheme { get; }
    public string Caption { get; }
}

public partial class ThemeSettings : ObservableObject
{
    [ObservableProperty] public ThemeInfo selectedTheme = ThemesList.First();

    public static List<ThemeInfo> ThemesList { get; } = new()
    {
        ThemeInfo.System,
        ThemeInfo.Light,
        ThemeInfo.Dark
    };

    public static ThemeSettings Current { get; } = new();

    partial void OnSelectedThemeChanged(ThemeInfo oldValue, ThemeInfo newValue)
    {
        Application.Current.UserAppTheme = newValue.AppTheme;
    }
}