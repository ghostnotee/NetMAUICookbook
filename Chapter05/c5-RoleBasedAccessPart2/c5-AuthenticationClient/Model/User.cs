namespace c5_AuthenticationClient.Model;

public class User
{
    public required string Email { get; set; }
    public DateOnly BirthDate { get; set; }
}