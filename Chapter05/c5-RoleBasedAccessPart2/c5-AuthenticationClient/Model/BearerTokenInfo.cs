namespace c5_AuthenticationClient.Model;

public class BearerTokenInfo
{
    public string AccessToken { get; set; } = null!;
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; } = null!;
    public DateTime? TokenTimestamp { get; set; }
}