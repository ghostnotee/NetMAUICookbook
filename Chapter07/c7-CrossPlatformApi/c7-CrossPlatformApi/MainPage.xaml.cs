﻿namespace c7_CrossPlatformApi;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        Label.Text = PlatformInfo.GetPlatform();
    }
}

public partial class PlatformInfo
{
    public static partial string GetPlatform();
}