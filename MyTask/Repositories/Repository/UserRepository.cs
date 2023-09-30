namespace MyTask;

public class UserRepository : BaseRepositorySetting<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    public async Task<User> GetByMail(string mail)
        => await dbSet.FirstOrDefaultAsync(e => e.Email == mail) 
        ?? throw new ArgumentException($"User was not found in over records.");

    public async Task DeleteByMail(string mail)
    {
        User userFromDb = await GetByMail(mail);

        await Task.Run(() => dbSet.Remove(userFromDb));
        await SaveChangesAsync();
    }

    public async Task<User> GetByToken(string token)
    {
        if (token.IsNullOrEmpty())
            throw new ArgumentException("Token was not provided");

        return await dbSet.FirstOrDefaultAsync(e => e.RefreshToken.Value == token) 
            ?? throw new ArgumentException($"Invalid Token.");
    }

    public async override Task Update(User entity)
    {
        await Task.Run(() => dbSet.Update(entity));
        await SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetUsersByAssignmentId(Guid assignmentId)
        => await dbSet.Include(e => e.UserAssignment)
                               .ThenInclude(e => e.Assignment)
                               .Where(e => e.UserAssignment.Any(e => e.AssignmentId == assignmentId))   
                               .AsSplitQuery()
                               .ToListAsync()
                                ?? throw new ArgumentException($"There is No Users Assigned To This Assignment");



    /*    public async Task<IEnumerable<Assignment>> GetAsssignmentsCreatedByUser(Guid userid)
            => await dbSet.Include(e => e.UserAssignment)
                          .ThenInclude(e => e.Assignment)
                          .SelectMany(e => e.UserAssignment.Where(e=>e.Assignment != null).Select(ua => ua.Assignment))
                          .Where(e => e.UserId == userid)
                          .ToListAsync();*/
}
