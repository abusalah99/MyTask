namespace MyTask;

public class AssignmentRepository : BaseRepositorySetting<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(ApplicationDbContext context) : base(context) { }
    public async Task<IEnumerable<Assignment>> GetAssignmentsCreatedByUser(Guid userId)
        => await dbSet.Include(e => e.UserAssignment)
                      .ThenInclude(e => e.User)
                      .Where(e=>e.UserId == userId)
                      .OrderBy(e=>e.DueTo)
                      .ToListAsync();
    public async Task<IEnumerable<Assignment>> GetAssignmentsWithUsersIncluded()
            => await dbSet.Include(e=>e.UserAssignment)
                          .ThenInclude(e=>e.User)
                          .OrderBy(e => e.DueTo)
                          .AsSingleQuery()
                          .ToListAsync();

    public async Task<IEnumerable<Assignment>> GetAssignmentsByUserId(Guid UserId)
    
    => await dbSet.Include(e => e.UserAssignment)
                           .ThenInclude(e => e.User)
                           .Where(e=>e.UserAssignment.Any(e=>e.UserId == UserId))
                           .OrderBy(e => e.DueTo)
                           .AsSplitQuery()
                           .ToListAsync()
                            ?? throw new ArgumentException($"There is No Assignments Assigned To This User");

}
