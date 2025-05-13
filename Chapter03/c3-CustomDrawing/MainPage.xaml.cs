namespace c3_CustomDrawing;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
}

public class BarChartDrawable : IDrawable
{
    Color[] Palette = [Colors.LightGreen, Colors.Gold, Colors.Coral];
    float spacing = 5;
    float cornerRadius = 4;
    public float Value { get; set; } = 1;

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        float rectSize = dirtyRect.Height;
        int maxStep = (int)(dirtyRect.Width / (rectSize + spacing));
        int valueBasedSteps = (int)(maxStep * Value);

        for (int step = 0; step < valueBasedSteps; step++)
        {
            canvas.FillColor = Palette[Palette.Length * step / maxStep];
            canvas.FillRoundedRectangle(
                x: (rectSize + spacing) * step,
                y: 0,
                width: rectSize,
                height: rectSize,
                cornerRadius: cornerRadius);
        }

        canvas.RestoreState();
    }
}