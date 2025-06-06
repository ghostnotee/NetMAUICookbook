using System.Text.Json;

namespace c5_AuthenticationClient.Model;

public class SessionService
{
    private static readonly string StorageKey = "TokenInfo";
    public static SessionService Instance { get; } = new();
    public BearerTokenInfo TokenInfo { get; private set; }

    public async Task SaveTokenToStorage(BearerTokenInfo token)
    {
        var tokenInfoString = JsonSerializer.Serialize(token);
        await SecureStorage.Default.SetAsync(StorageKey, tokenInfoString);
        TokenInfo = token;
    }

    public void ClearTokenStorage()
    {
        SecureStorage.Default.Remove(StorageKey);
        TokenInfo = null;
    }

    public async Task<bool> TokenExistsAsync()
    {
        if (TokenInfo != null)
            return true;
        var tokenString = await SecureStorage.Default.GetAsync(StorageKey);
        if (string.IsNullOrEmpty(tokenString))
            return false;
        TokenInfo = JsonSerializer.Deserialize<BearerTokenInfo>(tokenString);
        return true;
    }

    public bool TokenExpired()
    {
        return (DateTime.UtcNow - TokenInfo.TokenTimestamp.Value).TotalSeconds > TokenInfo.ExpiresIn;
    }

    public async Task<bool> UseExistingSession()
    {
        if (await TokenExistsAsync())
        {
            if (TokenExpired())
                TokenInfo = await WebService.Instance.RefreshTokenAsync(TokenInfo.RefreshToken);
            else
                WebService.Instance.SetAuthHeader(TokenInfo.AccessToken);
            return true;
        }

        return false;
    }
}