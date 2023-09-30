namespace MyTask;

public interface IUserRepository : IBaseRepositorySetting<User>
{
    Task<User> GetByMail(string mail);
    Task DeleteByMail(string mail);
    Task<User> GetByToken(string token);
    Task<IEnumerable<User>> GetUsersByAssignmentId(Guid assignmentId);
}
