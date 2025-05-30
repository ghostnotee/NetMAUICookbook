using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace c5_AuthenticationClient.Model;

public class WebService
{
    private const string BaseAddress = "https://zggwkbtz-7128.euw.devtunnels.ms/";

    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri(BaseAddress) };

    internal static WebService Instance { get; } = new();

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

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var response = await _httpClient.GetAsync("users");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task DeleteUserAsync(string email)
    {
        var response = await _httpClient.DeleteAsync($"users/delete/{email}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> CanDeleteUsersAsync()
    {
        var response = await _httpClient.GetAsync("users/candelete");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<bool>(json);
    }

    public async Task<User> GetCurrentUserAsync()
    {
        var response = await _httpClient.GetAsync("me");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task GoogleAuthAsync()
    {
        WebAuthenticatorResult authResult = await WebAuthenticator.Default.AuthenticateAsync(
            new Uri($"{BaseAddress}mauth/google"),
            new Uri("myapp://"));
        BearerTokenInfo tokenInfo = new BearerTokenInfo
        {
            AccessToken = authResult.AccessToken,
            RefreshToken = authResult.RefreshToken,
            ExpiresIn = int.Parse(authResult.Properties["expires_in"]),
            TokenTimestamp = DateTime.UtcNow
        };
        SetAuthHeader(tokenInfo.AccessToken);
    }
}