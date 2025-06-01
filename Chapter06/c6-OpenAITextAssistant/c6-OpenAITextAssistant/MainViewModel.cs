using System.ClientModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenAI.Chat;

namespace c6_OpenAITextAssistant;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private string? _letterText;

    private readonly ChatClient _aiClient = new("gpt-3.5-turbo",
        "[YOUR OPENAI API KEY HERE]");

    [RelayCommand]
    private async Task FixErrorsAsync()
    {
        try
        {
            ChatCompletion completion = await _aiClient.CompleteChatAsync(new List<ChatMessage>
            {
                new SystemChatMessage("You are an assistant correcting text"),
                new UserChatMessage($"Fix grammar errors: {LetterText}")
            });
            LetterText = completion?.Content[0].Text ?? string.Empty;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}