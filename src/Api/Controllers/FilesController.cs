using Api.Controllers.Base;
using Core.Queries;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authentication.Internal;
using SharedKernel.Authentication;
using SharedKernel.Dispatchers;

namespace Api.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : BaseController
{
    public FilesController
    (
        IDispatcher dispatcher
    ) : base(dispatcher)
    {
    }

    [Authorization(Permissions.ViewFiles)]
    [HttpGet]
    public async Task<IActionResult> GetFilesAsync([FromQuery] GetFilesQuery query, CancellationToken cancellationToken)
        => Ok(await _dispatcher.QueryAsync(query, cancellationToken));
}
