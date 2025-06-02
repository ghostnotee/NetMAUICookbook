using Microsoft.ML.Data;

namespace c6_AIFaceDetection;

public class NormalizationData
{
    [ColumnName("input")] [VectorType(3, 480, 640)]
    public VBuffer<float> Reshape;

    public static void MeanAndScaleNormalization(NormalizationData input, NormalizationData output)
    {
        output.Reshape = new VBuffer<float>(
            input.Reshape.Length,
            input.Reshape.GetValues().ToArray().Select(v => (v - 127) / 128).ToArray());
    }
}

public class ImageNetData
{
    public required string ImagePath { get; set; }
}