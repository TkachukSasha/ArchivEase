using Core.Dal;
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
) : ICommand<Result>;

public sealed class SignUpCommandHandler : ICommandHandler<SignUpCommand, Result>
{
    private readonly ArchivEaseContext _context;
    private readonly IPasswordManager _passwordManager;

    public SignUpCommandHandler
   (
       ArchivEaseContext context,
       IPasswordManager passwordManager
   )
    {
        _context = context;
        _passwordManager = passwordManager;
    }

    public async Task<Result> HandleAsync(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        User user = User.Init(command.UserName, _passwordManager.Secure(command.Password)).Value;

        _ = _context
            .Users
            .Add(user);

        Role? role = await _context
            .Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == Role.DefaultRole, cancellationToken);

        if(role is null)
        {
            return Result.Failure(RoleErrors.RoleNotFound);
        }

        UserRole userRole = UserRole.Init(user.Id, role!.Id).Value;

        _ = _context
            .UserRoles
            .Add(userRole);

        return Result.Success();
    }
}