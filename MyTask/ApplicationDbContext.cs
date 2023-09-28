namespace MyTask;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration())
                    .ApplyConfiguration(new AssignmentConfiguration())
                    .ApplyConfiguration(new UserAssignmentConfiguration());

    }
}
