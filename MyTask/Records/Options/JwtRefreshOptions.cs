namespace MyTask;

public record JwtRefreshOptions 
{
    public string SecretKey { get; init; } = null!;
    public int ExpireTimeInMonths { get; init; }
}