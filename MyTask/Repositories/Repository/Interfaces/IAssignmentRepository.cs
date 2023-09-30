using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MyTask;

public interface IAssignmentRepository : IBaseRepositorySetting<Assignment> 
{
    Task<IEnumerable<Assignment>> GetAssignmentsCreatedByUser(Guid userId);
    Task<IEnumerable<Assignment>> GetAssignmentsByUserId(Guid UserId);

    Task<IEnumerable<Assignment>> GetAssignmentsWithUsersIncluded();
}
