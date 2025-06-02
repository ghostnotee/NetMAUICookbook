using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.ML;
using Microsoft.ML.Data;
using SkiaSharp;
using static Microsoft.ML.Transforms.Image.ImageResizingEstimator;

namespace c6_AIFaceDetection;

public partial class MainViewModel : ObservableObject
{
    private readonly string _modelName = "version-RFB-640.onnx";
    private readonly string _sourceImageName = "Photo.jpg";

    private readonly MLContext mlContext = new();

    [ObservableProperty] private ImageSource? _faceImage;
    private ITransformer _mlModel;

    [ObservableProperty] private ImageSource? _sourceImage;

    private string _sourceImagePath;

    private ITransformer LoadModel(string modelLocation)
    {
        var data = mlContext.Data.LoadFromEnumerable(new List<ImageNetData>());
        return mlContext.Transforms.LoadImages(
                "input", "",
                nameof(ImageNetData.ImagePath))
            .Append(mlContext.Transforms.ResizeImages(
                "input",
                320,
                240,
                "input",
                ResizingKind.Fill))
            .Append(mlContext.Transforms.ExtractPixels("input"))
            .Append(mlContext.Transforms.CustomMapping<NormalizationData, NormalizationData>(
                NormalizationData.MeanAndScaleNormalization,
                null))
            .Append(mlContext.Transforms.ApplyOnnxModel(
                modelFile: modelLocation,
                outputColumnNames: new[] { "scores", "boxes" },
                inputColumnNames: new[] { "input" }))
            .Fit(data);
    }

    [RelayCommand]
    private async Task InitializeAsync()
    {
        _sourceImagePath = await FileOperations.CopyToAppData(_sourceImageName);
        var onnxModelPath = await FileOperations.CopyToAppData(_modelName);

        SourceImage = ImageSource.FromFile(_sourceImagePath);
        _mlModel = LoadModel(onnxModelPath);
    }

    private IDataView GetScoredData(string imagePath)
    {
        IEnumerable<ImageNetData> images = new List<ImageNetData> { new() { ImagePath = imagePath } };
        var imageDataView = mlContext.Data.LoadFromEnumerable(images);
        return _mlModel.Transform(imageDataView);
    }

    private int GetFaceBoxIndex(float[] scores)
    {
        int objectScoreIndex;
        float maxScore = -1;
        var maxScoreIndex = 0;
        for (var i = 0; i < scores.Length / 2; i++)
        {
            objectScoreIndex = i * 2 + 1;
            if (scores[objectScoreIndex] > maxScore)
            {
                maxScore = scores[objectScoreIndex];
                maxScoreIndex = i;
            }
        }

        return maxScoreIndex;
    }

    [RelayCommand]
    private async Task FindFaceAsync()
    {
        await Task.Run(() =>
        {
            var scoredData = GetScoredData(_sourceImagePath);
            var boxes = scoredData.GetColumn<float[]>("boxes").First();
            var scores = scoredData.GetColumn<float[]>("scores").First();

            var scoreIndex = GetFaceBoxIndex(scores);
            var boxIndex = scoreIndex * 4;

            CropImage(_sourceImagePath,
                boxes[boxIndex],
                boxes[boxIndex + 1],
                boxes[boxIndex + 2],
                boxes[boxIndex + 3]);
        });
    }

    private void CropImage(string imagePath, float left, float top, float right, float bottom)
    {
        using var inputStream = File.OpenRead(imagePath);
        var originalBitmap = SKBitmap.Decode(inputStream);
        var croppedBitmap = CreateCroppedBitmap(
            originalBitmap,
            new SKRectI(
                (int)(left * originalBitmap.Width),
                (int)(top * originalBitmap.Height),
                (int)(right * originalBitmap.Width),
                (int)(bottom * originalBitmap.Height)));
        FaceImage = ImageSource.FromStream(() => BitmapToStream(croppedBitmap));
    }

    private SKBitmap CreateCroppedBitmap(SKBitmap originalBitmap, SKRectI cropRect)
    {
        var croppedBitmap = new SKBitmap(cropRect.Width, cropRect.Height);
        using var canvas = new SKCanvas(croppedBitmap);
        canvas.DrawBitmap(originalBitmap, cropRect, new SKRectI(0, 0, cropRect.Width, cropRect.Height));
        return croppedBitmap;
    }

    private Stream BitmapToStream(SKBitmap bitmap)
    {
        var memoryStream = new MemoryStream();
        bitmap.Encode(memoryStream, SKEncodedImageFormat.Jpeg, 30);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }
}