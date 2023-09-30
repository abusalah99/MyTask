namespace MyTask;

public class User : BaseEntitySetting
{
    public string Email { get; set; } = null!;
    [JsonIgnore]
    public string Password { get; set; } = null!;
    public string Phone {  get; set; } = null!; 
    public string? ProfilePhotoUrl { get; set; }
    [JsonIgnore]
    public string Role { get; set; } = null!;
    [JsonIgnore]
    public RefreshToken RefreshToken { get; set; } = new ();
    [JsonIgnore]
    public List<Assignment> Assignment { get; set; } = new ();
    [JsonIgnore]
    public List<UserAssignment> UserAssignment { get; set; } = new ();
}
