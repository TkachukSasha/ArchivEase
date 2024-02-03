using Core.Dal;
using Core.Dtos;
using Core.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Authentication;
using SharedKernel.Commands;
using SharedKernel.Errors;

namespace Core.Commands;

public sealed record SignUpCommand
(
    string UserName,
    string Password
) : ICommand<Result<UserResponseDto>>;

internal sealed class SignUpCommandHandler : ICommandHandler<SignUpCommand, Result<UserResponseDto>>
{
    private readonly ArchivEaseContext _context;
    private readonly IPasswordManager _passwordManager; 
    private readonly IJwtTokenProvider _jwtTokenProvider;

    public SignUpCommandHandler
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

    public async Task<Result<UserResponseDto>> HandleAsync(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        User user = User.Init(command.UserName, _passwordManager.Secure(command.Password)).Value;

        _ = _context
            .Users
            .Add(user);

        Role? role = await _context
            .Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == "user", cancellationToken);

        if(role is null)
        {
            return Result.Failure<UserResponseDto>(RoleErrors.RoleNotFound);
        }

        UserRole userRole = UserRole.Init(user.Id, role!.Id).Value;

        _ = _context
            .UserRoles
            .Add(userRole);

        var permissionValue = (int)role.Permissions;

        var token = _jwtTokenProvider.CreateToken
        (
            user.Id.Value.ToString(),
            user.UserName,
            permissionValue.ToString()
        );

        return Result.Success(new UserResponseDto(user.Id, user.UserName, false, token.Token));
    }
}