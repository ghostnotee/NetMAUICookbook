namespace c5_AuthenticationClient.Model;

public class BearerTokenInfo
{
    public string? AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? TokenTimestamp { get; set; }
}