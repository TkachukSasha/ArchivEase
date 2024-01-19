using Core.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions;
using SharedKernel.Authentication;

namespace Core.Dal.Configurations;

internal sealed class UsersConfigurations :
    IEntityTypeConfiguration<User>,
    IEntityTypeConfiguration<Role>,
    IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new UserId(x));

        builder.Property(x => x.UserName)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(x => x.Password)
            .IsRequired();

        builder.Property(x => x.RecordState)
            .HasDefaultValue(RecordState.Active);

        builder
            .HasIndex(x => x.UserName, "idx_users_user_name");
    }

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new RoleId(x));

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Permissions)
            .HasDefaultValue(Permissions.None);

        builder.Property(x => x.RecordState)
            .HasDefaultValue(RecordState.Active);
    }

    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles");

        builder.HasKey(x => new { x.UserId, x.RoleId });
    }
}