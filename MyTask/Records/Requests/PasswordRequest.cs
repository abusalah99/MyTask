namespace MyTask;

public record PasswordRequest 
{
    public string NewPassword { get; set; } = null!;
    public string Password { get; set; } = null!;

}