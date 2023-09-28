namespace MyTask;

public class User : BaseEntitySetting
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Phone {  get; set; } = null!; 
    public string? ProfilePhotoUrl { get; set; }
    public string Role { get; set; } = null!;
    public RefreshToken? RefreshToken { get; set; }
    public List<Assignment> Assignment { get; set; } = new ();
    public List<UserAssignment> UserAssignment { get; set; } = new ();
}
