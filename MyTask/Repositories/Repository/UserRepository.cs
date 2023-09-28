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
        if (token == null)
            throw new ArgumentException("Token was not provided");

        return await dbSet.FirstOrDefaultAsync(e => e.RefreshToken.Value == token) 
            ?? throw new ArgumentException($"Invalid Token.");
    }

}