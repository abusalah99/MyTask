namespace MyTask;

public class UserConfiguration : BaseConfigrationSettings<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => x.Email).IsUnique();

        builder.OwnsOne(e => e.RefreshToken, refreshTokenBuilder =>
        {
            refreshTokenBuilder.Property(e => e.Value).IsRequired().HasMaxLength(128);

            refreshTokenBuilder.Property(e => e.CreatedAt).IsRequired();

            refreshTokenBuilder.Property(e => e.ExpireAt).IsRequired();
        });

        builder.Property(e => e.Password).IsRequired();

        builder.Property(e => e.Email).IsRequired();

        builder.Property(e => e.Phone).IsRequired().HasMaxLength(15);

        builder.HasMany(e => e.Assignment).WithOne(e => e.User).HasForeignKey(e => e.UserId).IsRequired();
        builder.HasMany(e => e.UserAssignment).WithOne(e => e.User).HasForeignKey(e => e.UserId).IsRequired();  
    }
}