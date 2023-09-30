namespace MyTask;

public interface IAssignmentUnitOfWork : IBaseUnitOfWorkSetting<Assignment>
{
    Task<IEnumerable<object>> GetAssignmentsCreatedByUser();
    Task SetUserAssignmentAsFinishedForAdmin(Guid assignmentId, Guid userId );
    Task SetUserAssignmentAsFinished(Guid assignmentId);
    Task<IEnumerable<object>> GetAllAssignmentsForAdmin();
    Task<List<object>> GetAssignmentAssignedToUser();
}
