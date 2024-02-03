using Core.Dal;
using Core.Dtos;
using Core.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Authentication;
using SharedKernel.Commands;
using SharedKernel.Errors;

namespace Core.Commands;

public record SignInCommand
(
    string UserName,
    string Password
) : ICommand<Result<UserResponseDto>>;

internal sealed class SignInCommandHandler : ICommandHandler<SignInCommand, Result<UserResponseDto>>
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

    public async Task<Result<UserResponseDto>> HandleAsync(SignInCommand command, CancellationToken cancellationToken = default)
    {
        User? user = await _context
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponseDto>(UserErrors.UserIsNotFound(command.UserName));
        }

        bool isValidPassword = _passwordManager.Validate(command.Password, user.Password);

        if (!isValidPassword)
        {
            return Result.Failure<UserResponseDto>(UserErrors.UserPasswordIsNotValid(command.Password));
        }

        var (isAnyAdminRole, permissions) = await GetUserRoleInfoAsync(user.Id, cancellationToken);

        var permissionValue = (int)permissions;

        var token = _jwtTokenProvider.CreateToken
        (
            user.Id.Value.ToString(),
            user.UserName,
            permissionValue.ToString()
        );

        return Result.Success(new UserResponseDto(user.Id, user.UserName, isAnyAdminRole, token.Token));
    }

    private async Task<(bool, Permissions)> GetUserRoleInfoAsync
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

        var isAnyAdminRole = rolesPermissions
            .Any(x => x.Name == Role.AdminRole);

        return 
        (
            isAnyAdminRole, 
            rolesPermissions
                .Select(role => role.Permissions)
                .Aggregate((current, next) => current | next)
        );
    }
}