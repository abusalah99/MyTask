namespace MyTask;

public record JwtAccessOptions
{
    public  string? Issuser { get; init; } 
    public  string? Audience { get; init; }
    public string SecretKey { get; init; } = null!;
    public int ExpireTimeInMintes { get; init; }
}