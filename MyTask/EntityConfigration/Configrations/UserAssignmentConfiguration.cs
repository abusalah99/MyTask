namespace MyTask;

public class UserAssignmentConfiguration : IEntityTypeConfiguration<UserAssignment> 
{
    public void Configure(EntityTypeBuilder<UserAssignment> builder)
    {
        builder.HasKey(e => new { e.UserId, e.AssignmentId }); ;

        builder.Property(e => e.UserId).IsRequired();

        builder.Property(e => e.AssignmentId).IsRequired();

    }
}
