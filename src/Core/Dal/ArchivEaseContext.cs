using Core.Encodings;
using Core.Users;
using Microsoft.EntityFrameworkCore;

namespace Core.Dal;

internal sealed class ArchivEaseContext : DbContext
{
    internal DbSet<EncodingTable> EncodingTables => Set<EncodingTable>();

    internal DbSet<EncodingAlgorithm> EncodingAlgorithms => Set<EncodingAlgorithm>();

    internal DbSet<EncodingLanguage> EncodingLanguages => Set<EncodingLanguage>();

    internal DbSet<User> Users => Set<User>();

    internal DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArchivEaseContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
