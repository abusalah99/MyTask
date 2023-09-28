namespace MyTask;

public class UserAssignment
{
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    public Assignment Assignment { get; set; } = null!;
    public Guid AssignmentId { get; set; }
}
