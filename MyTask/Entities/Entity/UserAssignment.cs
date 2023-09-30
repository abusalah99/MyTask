namespace MyTask;

public class UserAssignment
{
    public User? User { get; set; } 
    public Guid UserId { get; set; }
    public Assignment? Assignment { get; set; } 
    public Guid AssignmentId { get; set; }
    public string Status { get; set; } = "Unfinished";
    public DateTime? FinishedAt { get; set; } = null;    
}
