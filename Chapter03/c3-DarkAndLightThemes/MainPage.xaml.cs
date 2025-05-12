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
	public AppTheme AppTheme { get; }
	public string Caption { get; }

	public ThemeInfo(AppTheme theme, string caption)
	{
		AppTheme = theme;
		Caption = caption;
	}
}