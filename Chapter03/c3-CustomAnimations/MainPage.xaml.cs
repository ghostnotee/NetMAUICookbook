using CommunityToolkit.Maui.Animations;

namespace c3_CustomAnimations;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
}

internal class AttentionAnimation : BaseAnimation
{
    public override async Task Animate(VisualElement view, CancellationToken token = default)
    {
        for (var i = 0; i < 6; i++)
        {
            await view.FadeTo(0.5, Length, Easing);
            await view.FadeTo(1, Length, Easing);
        }
    }
}