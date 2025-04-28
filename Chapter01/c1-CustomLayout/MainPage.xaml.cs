using Microsoft.Maui.Layouts;

namespace c1_CustomLayout;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
}

public class CircularLayout : Layout
{
    public static readonly BindableProperty RadiusProperty = BindableProperty.Create("Radius", typeof(double), typeof(CircularLayout));

    public double Radius
    {
        get { return (double)GetValue(RadiusProperty); }
        set { SetValue(RadiusProperty, value); }
    }

    protected override ILayoutManager CreateLayoutManager()
    {
        return new CircularLayoutManager(this);
    }
}

public class CircularLayoutManager : ILayoutManager
{
    private readonly CircularLayout parentLayout;

    public CircularLayoutManager(CircularLayout layout)
    {
        parentLayout = layout;
    }

    public Size Measure(double widthConstraint, double heightConstraint)
    {
        var radius = parentLayout.Radius;
        for (var n = 0; n < parentLayout.Count; n++)
        {
            var child = parentLayout[n];
            if (child.Visibility == Visibility.Collapsed) continue;
            child.Measure(double.PositiveInfinity, double.PositiveInfinity);
        }

        return new Size(parentLayout.WidthRequest, parentLayout.HeightRequest);
    }

    public Size ArrangeChildren(Rect bounds)
    {
        var radius = parentLayout.Radius;
        var angleStep = Math.PI * 2 / parentLayout.Count;
        for (var i = 0; i < parentLayout.Count; i++)
        {
            var child = parentLayout[i];
            if (child.Visibility == Visibility.Collapsed) continue;
            child.Arrange(new Rect(
                radius * Math.Cos(angleStep * i) + radius,
                radius * Math.Sin(angleStep * i) + radius,
                child.DesiredSize.Width,
                child.DesiredSize.Height));
        }

        return new Size(parentLayout.WidthRequest, parentLayout.HeightRequest);
    }
}