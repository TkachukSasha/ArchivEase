using Api.Controllers.Base;
using Core.Queries;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authentication.Internal;
using SharedKernel.Authentication;
using SharedKernel.Dispatchers;
using Core.Commands;
using SharedKernel.Errors;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : BaseController
{
    public UsersController
    (
        IDispatcher dispatcher
    ) : base(dispatcher)
    {
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync([FromBody] SignInCommand command, CancellationToken cancellationToken)
    {
        Result result = await _dispatcher.SendAsync(command, cancellationToken);

        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUpAsync([FromBody] SignUpCommand command, CancellationToken cancellationToken)
    {
        Result result = await _dispatcher.SendAsync(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest();
    }

    [Authorization(Permissions.ViewUsers)]
    [HttpGet]
    public async Task<IActionResult> GetUsersAsync([FromQuery] GetUsersQuery query, CancellationToken cancellationToken)
        => Ok(await _dispatcher.QueryAsync(query, cancellationToken));

    [Authorization(Permissions.ViewUsers)]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        GetUserQuery query = new(userId);

        return Ok(await _dispatcher.QueryAsync(query, cancellationToken));
    }
}
