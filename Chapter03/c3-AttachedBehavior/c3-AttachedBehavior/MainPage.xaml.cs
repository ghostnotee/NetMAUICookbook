namespace c3_AttachedBehavior;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
}

public class DoubleTapToZoomBehavior : Behavior<Image>
{
    public static readonly BindableProperty ScaleFactorProperty =
        BindableProperty.Create("ScaleFactor", typeof(double), typeof(DoubleTapToZoomBehavior), 2d);

    private Image image;
    private bool isZoomed;
    private TapGestureRecognizer tapGestureRecognizer;

    public double ScaleFactor
    {
        get { return (double)GetValue(ScaleFactorProperty); }
        set { SetValue(ScaleFactorProperty, value); }
    }

    protected override void OnAttachedTo(Image bindable)
    {
        base.OnAttachedTo(bindable);
        tapGestureRecognizer = new TapGestureRecognizer
        {
            NumberOfTapsRequired = 2
        };
        image = bindable;
        tapGestureRecognizer.Tapped += OnImageDoubleTap!;
        image.GestureRecognizers.Add(tapGestureRecognizer);
    }

    protected override void OnDetachingFrom(Image bindable)
    {
        base.OnDetachingFrom(bindable);
        image.GestureRecognizers.Remove(tapGestureRecognizer);
        tapGestureRecognizer.Tapped -= OnImageDoubleTap;
        image = null!;
    }

    private void OnImageDoubleTap(object sender, TappedEventArgs e)
    {
        var tappedPoint = e.GetPosition(image);
        if (isZoomed)
        {
            image.ScaleTo(1);
            image.TranslateTo(0, 0);
        }
        else
        {
            var translateFactor = ScaleFactor - 1;
            var traslateX = (image.Width / 2 - tappedPoint!.Value.X) * translateFactor;
            var traslateY = (image.Height / 2 - tappedPoint.Value.Y) * translateFactor;
            image.TranslateTo(traslateX, traslateY);
            image.ScaleTo(ScaleFactor);
        }

        isZoomed = !isZoomed;
    }
}