namespace MyTask;

public class AssignmentConfiguration : BaseConfigrationSettings<Assignment>
{
    public override void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.Property(e => e.Description).IsRequired();

        builder.Property(e => e.DueTo).IsRequired();

        builder.Property(e => e.UserId).IsRequired();

        builder.HasMany(e => e.UserAssignment).WithOne(e => e.Assignment).HasForeignKey(e => e.AssignmentId).IsRequired();
    }
}
