using SkiaSharp;

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

    public Stream ResizeImage(Stream inputStream, int width, int height)
    {
        using SKBitmap? originalBitmap = SKBitmap.Decode(inputStream);

        using SKBitmap resizedBitmap = new(width, height);
        using SKCanvas canvas = new(resizedBitmap);
        canvas.Clear(SKColors.Transparent);

        var scale = Math.Min((float)width / originalBitmap.Width, (float)height / originalBitmap.Height);
        SKRect destRect = new(0, 0, originalBitmap.Width * scale, originalBitmap.Height * scale);
        canvas.DrawBitmap(originalBitmap, destRect);

        MemoryStream outputStream = new();
        resizedBitmap.Encode(outputStream, SKEncodedImageFormat.Png, 100);
        outputStream.Seek(0, SeekOrigin.Begin);

        return outputStream;
    }
}