using Core.Encodings;
using Core.Users;
using Microsoft.EntityFrameworkCore;

namespace Core.Dal;

public class ArchivEaseContext : DbContext
{
    public ArchivEaseContext() { }

    public ArchivEaseContext(DbContextOptions<ArchivEaseContext> options) : base(options) { }

    public DbSet<EncodingTable> EncodingTables => Set<EncodingTable>();

    public DbSet<EncodingAlgorithm> EncodingAlgorithms => Set<EncodingAlgorithm>();

    public DbSet<EncodingLanguage> EncodingLanguages => Set<EncodingLanguage>();

    public DbSet<EncodingFile> EncodingFiles => Set<EncodingFile>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArchivEaseContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
