namespace MyTask;

public record RefreshToken 
{
    public required string Value { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime ExpireAt { get; set; }
}