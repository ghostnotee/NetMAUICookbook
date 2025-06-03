using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c6_FileUploadingClient;

public partial class MainViewModel : ObservableObject
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://zggwkbtz-7128.euw.devtunnels.ms/")
    };

    [ObservableProperty] private string _fileName;


    [ObservableProperty] private string _textProgress;

    [ObservableProperty] private double _uploadProgress;

    private async Task<HttpResponseMessage> SendChunkAsync(byte[] chunkBuffer, int bytesRead, int chunkNumber, int totalChunks)
    {
        var content = new ByteArrayContent(chunkBuffer, 0, bytesRead);

        content.Headers.Add("Chunk-Number", chunkNumber.ToString());
        content.Headers.Add("Total-Chunks", totalChunks.ToString());
        content.Headers.Add("File-Name", FileName);

        var response = await _httpClient.PostAsync("upload", content);
        response.EnsureSuccessStatusCode();
        return response;
    }

    [RelayCommand]
    private async Task UploadFileAsync()
    {
        var result = await FilePicker.Default.PickAsync();
        if (result == null) return;
        FileName = result.FileName;

        const int chunkSize = 2 * 1024 * 1024;
        var fileStream = new FileStream(result.FullPath, FileMode.Open, FileAccess.Read);
        var fileLength = fileStream.Length;
        var totalChunks = (int)Math.Ceiling((double)fileLength / chunkSize);
        TextProgress = "Starting...";
        for (var chunkNumber = 0; chunkNumber < totalChunks; chunkNumber++)
        {
            var chunkBuffer = new byte[chunkSize];
            var bytesRead = await fileStream.ReadAsync(chunkBuffer, 0, chunkSize);

            await SendChunkAsync(chunkBuffer, bytesRead, chunkNumber, totalChunks);

            UploadProgress = (double)(chunkNumber + 1) / totalChunks;
            TextProgress = $"{chunkNumber + 1} / {totalChunks}";
        }

        TextProgress = "Uploaded";
    }
}