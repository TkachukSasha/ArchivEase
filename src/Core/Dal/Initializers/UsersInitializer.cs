using Core.Dal.Initializers.Internal;
using Core.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.Authentication;
using SharedKernel.Dal;

namespace Core.Dal.Initializers;

internal sealed class UsersInitializer : BaseInitializer<ArchivEaseContext, UsersInitializer>, IDataInitializer
{
    private readonly IPasswordManager _passwordManager;

    internal UsersInitializer(ArchivEaseContext context, ILogger<UsersInitializer> logger, IPasswordManager passwordManager)
        : base(context, logger)
    {
        _passwordManager = passwordManager;
    }

    public async Task InitAsync()
    {
        Guid userId = Guid.Empty;

        if (!await _context.Users.AnyAsync())
            userId = await InitUsersAsync();

        if (!await _context.Roles.AnyAsync())
            await InitRolesAsync(userId);

        await _context.SaveChangesAsync();
    }

    private async Task<Guid> InitUsersAsync()
    {
        var user = User.Init("@CrySoul1l", _passwordManager.Secure("1337Master")).Value;

        await _context.Users.AddAsync(user);

        _logger.Log(LogLevel.Information, "users initialized succesessfully!");

        return user.Id.Value;
    }

    private async Task InitRolesAsync(Guid userId)
    {
        var adminRole = Role.Init("admin", Permissions.All, userId).Value;

        var userRole = Role.Init("user", Permissions.ManageAccountInformation | Permissions.EncodeFile | Permissions.DecodeFile, userId).Value;

        await _context.Roles.AddAsync(adminRole);
        await _context.Roles.AddAsync(userRole);

        _logger.LogInformation("roles initialized succesessfully!");
    }
}