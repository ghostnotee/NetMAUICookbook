namespace c3_CustomDrawing;

public partial class SegmentedBarChartView : ContentView
{
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create("Value",
            typeof(float),
            typeof(SegmentedBarChartView),
            0f,
            propertyChanged: (b, o, n) => ((SegmentedBarChartView)b).OnValueChanged());

    public SegmentedBarChartView()
    {
        InitializeComponent();
    }

    public float Value
    {
        get { return (float)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    private void OnValueChanged()
    {
        ((BarChartDrawable)GraphicsView.Drawable).Value = Value;
        GraphicsView.Invalidate();
    }
}