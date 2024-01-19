using Core.Dal;
using Core.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Authentication;
using SharedKernel.Commands;
using SharedKernel.Errors;
using System.Threading;

namespace Core.Commands;

public record SignInCommand
(
    string UserName,
    string Password
) : ICommand<Result>;

public sealed class SignInCommandHandler : ICommandHandler<SignInCommand, Result>
{
    private readonly ArchivEaseContext _context;
    private readonly IPasswordManager _passwordManager;
    private readonly IJwtTokenProvider _jwtTokenProvider;

    public SignInCommandHandler
    (
        ArchivEaseContext context,
        IPasswordManager passwordManager,
        IJwtTokenProvider jwtTokenProvider
    )
    {
        _context = context;
        _passwordManager = passwordManager;
        _jwtTokenProvider = jwtTokenProvider;
    }

    public async Task<Result> HandleAsync(SignInCommand command, CancellationToken cancellationToken = default)
    {
        User? user = await _context
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken);

        if (user is null)
        {
            return Result.Failure<JsonAccessToken>(UserErrors.UserIsNotFound(command.UserName));
        }

        bool isValidPassword = _passwordManager.Validate(command.Password, user.Password);

        if (!isValidPassword)
        {
            return Result.Failure<JsonAccessToken>(UserErrors.UserPasswordIsNotValid(command.Password));
        }

        var permissionValue = (int)await GetUserRolePermissionsAsync(user.Id, cancellationToken);

        var token = _jwtTokenProvider.CreateToken
        (
            user.Id.Value.ToString(),
            user.UserName,
            permissionValue.ToString()
        );

        return Result.Success(token);
    }

    private async Task<Permissions> GetUserRolePermissionsAsync
    (
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        List<Guid> userRolesIds = await _context
           .UserRoles
           .AsNoTracking()
           .Where(x => x.UserId == userId)
           .Select(x => x.RoleId)
           .ToListAsync(cancellationToken);

        IQueryable<Role> roles = _context
            .Roles
            .AsNoTracking();

        var rolesPermissions = await roles
            .Where(r => userRolesIds.Contains(r.Id))
            .ToListAsync(cancellationToken);

        return rolesPermissions
            .Select(role => role.Permissions)
            .Aggregate((current, next) => current | next);
    }
}