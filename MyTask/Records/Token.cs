namespace MyTask;

public record Token
{
    public string AccessToken { get; set; } = null!;
    public DateTime AccessTokenExpiresAt { get; set; }
    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpiresAtExpires { get; set; }
    public string Role { get; set; } = null!;
}
