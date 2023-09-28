namespace MyTask;

public class Assignment : BaseEntitySetting
{
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime DueTo { get; set; }
    public User Creator { get; set; } = null!;
    public Guid UserId { get; set; }
    public List<UserAssignment> UserAssignment { get; set; } = new ();
}
