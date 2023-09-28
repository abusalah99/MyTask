namespace MyTask;

public record LoginRequest
{
    public required string Email { get; set; }
    public required string password { get; set; }

}
