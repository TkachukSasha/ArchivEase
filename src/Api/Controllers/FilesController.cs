using Api.Controllers.Base;
using Core.Queries;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authentication.Internal;
using SharedKernel.Authentication;
using SharedKernel.Dispatchers;
using Core.Commands;
using Core.Dtos;

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

    [HttpPost("encode")]
    public async Task<IActionResult> EncodeFilesAsync([FromForm] List<IFormFile> files, CancellationToken cancellationToken)
    {
        var command = new EncodeCommand
         (
             files.Select(x => new FileEntryDto(x.FileName, x.ContentType, x.Length, x.OpenReadStream())).ToList()
         );

        return Ok(await _dispatcher.SendAsync(command, cancellationToken));
    }

    [HttpPost("encodev2")]
    public async Task<IActionResult> EncodeAsync([FromBody] EncodeCommand command, CancellationToken cancellationToken)
        => Ok(await _dispatcher.SendAsync(command, cancellationToken));

    [HttpPost("decodev2")]
    public async Task<IActionResult> DecodeAsync([FromBody] DecodeCommand command, CancellationToken cancellationToken)
    => Ok(await _dispatcher.SendAsync(command, cancellationToken));

    [Authorization(Permissions.ViewFiles)]
    [HttpGet]
    public async Task<IActionResult> GetFilesAsync([FromQuery] GetFilesQuery query, CancellationToken cancellationToken)
        => Ok(await _dispatcher.QueryAsync(query, cancellationToken));
}
