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

    public UsersInitializer(ArchivEaseContext context, ILogger<UsersInitializer> logger, IPasswordManager passwordManager)
        : base(context, logger)
    {
        _passwordManager = passwordManager;
    }

    public async Task InitAsync()
    {
        if (await _context.Users.AnyAsync()) return;

        await InitUsersAsync();

        await _context.SaveChangesAsync();
    }

    private async Task InitUsersAsync()
    {
        var user = User.Init("@CrySoul1l", _passwordManager.Secure("1337Master")).Value;

        List<Role> roles = new() 
        { 
            Role.Init("admin", Permissions.All).Value,
            Role.Init("user", Permissions.ManageAccountInformation | Permissions.EncodeFile | Permissions.DecodeFile | Permissions.ViewFiles).Value
        };

        await _context.Users.AddAsync(user);

        await _context.Roles.AddRangeAsync(roles);

        foreach(var role in roles)
        {
            await _context.UserRoles.AddAsync(UserRole.Init(user.Id, role.Id).Value);
        }

        _logger.Log(LogLevel.Information, "users initialized succesessfully!");
    }
}