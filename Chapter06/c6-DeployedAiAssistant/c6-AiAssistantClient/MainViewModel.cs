using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c6_AiAssistantClient;

public partial class MainViewModel : ObservableObject
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://zggwkbtz-7128.euw.devtunnels.ms/"),
        Timeout = TimeSpan.FromSeconds(30)
    };

    [ObservableProperty] private string? _answer;

    [NotifyCanExecuteChangedFor(nameof(SendMessageCommand))] [ObservableProperty]
    private string? _message;

    [RelayCommand(CanExecute = nameof(CanSendMessage))]
    private async Task SendMessageAsync()
    {
        var response = await _httpClient.GetAsync($"ask-question?question='{Uri.EscapeDataString(Message)}'");
        response.EnsureSuccessStatusCode();
        Answer = await response.Content.ReadAsStringAsync();
        Message = null;
    }

    private bool CanSendMessage() => !string.IsNullOrEmpty(Message);
}