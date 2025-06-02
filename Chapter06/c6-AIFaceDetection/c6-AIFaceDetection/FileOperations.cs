namespace c6_AIFaceDetection;

public static class FileOperations
{
    public static async Task<string> CopyToAppData(string assetFileName)
    {
        var targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, assetFileName);
        if (File.Exists(targetFile))
            return targetFile;
        using var inputStream = await FileSystem.Current.OpenAppPackageFileAsync(assetFileName);
        using var outputStream = File.Create(targetFile);
        await inputStream.CopyToAsync(outputStream);
        return targetFile;
    }
}