using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace c5_AuthenticationClient.Model;

public class WebService
{
    private const string BaseAddress = "https://zggwkbtz-7128.euw.devtunnels.ms/";

    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri(BaseAddress) };

    public static WebService Instance { get; } = new();

    public async Task<BearerTokenInfo> Authenticate(string email, string password)
    {
        return await RequestTokenAsync("login/", new { email, password });
    }

    private async Task<BearerTokenInfo> RequestTokenAsync(string url, object postContent)
    {
        var response =
            await _httpClient.PostAsync(url, new StringContent(JsonSerializer.Serialize(postContent), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();

        var tokenInfo = (await response.Content.ReadFromJsonAsync<BearerTokenInfo>())!;
        tokenInfo.TokenTimestamp = DateTime.UtcNow;
        SetAuthHeader(tokenInfo.AccessToken!);
        return tokenInfo;
    }

    private void SetAuthHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}