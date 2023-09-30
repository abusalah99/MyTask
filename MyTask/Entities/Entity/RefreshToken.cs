namespace MyTask;

public record RefreshToken 
{
    public  string Value { get; set; } = string.Empty;
    public  DateTime CreatedAt { get; set; } = DateTime.MinValue;
    public DateTime ExpireAt { get; set; } = DateTime.MinValue;
}