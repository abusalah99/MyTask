namespace MyTask;

public class Assignment : BaseEntitySetting
{
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime DueTo { get; set; }
    public User? User { get; set; }
    public Guid UserId { get; set; }
    public required List<UserAssignment> UserAssignment { get; set; } 
}
