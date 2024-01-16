using Core.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions;

namespace Core.Dal.Configurations;

internal sealed class UsersConfigurations :
    IEntityTypeConfiguration<User>,
    IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new UserId(x));

        builder.Property(x => x.RecordState).HasDefaultValue(RecordState.Active);
    }

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new RoleId(x));

        builder.Property(x => x.RecordState).HasDefaultValue(RecordState.Active);

        builder.HasOne(x => x.User)
            .WithMany();
    }
}