using Api.Controllers.Base;
using Core.Queries;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authentication.Internal;
using SharedKernel.Authentication;
using SharedKernel.Dispatchers;
using Core.Commands;
using Core.Dtos;
using SharedKernel.Files;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : BaseController
{
    private readonly IFileGetter _fileGetter;

    public FilesController
    (
        IDispatcher dispatcher,
        IFileGetter fileGetter
    ) : base(dispatcher)
    {
        _fileGetter = fileGetter;
    }

    [Authorization(Permissions.EncodeFiles | Permissions.DecodeFiles)]
    [HttpPost("encode")]
    public async Task<IActionResult> EncodeFilesAsync([FromForm] List<IFormFile> files, [FromQuery] Guid userId, CancellationToken cancellationToken)
    {
        var command = new EncodeCommand
        (
            userId,
            files.Select(x => new FileEntryDto(x.FileName, x.ContentType, x.Length, x.OpenReadStream())).ToList()
        );

        var result = await _dispatcher.SendAsync(command, cancellationToken);

        return File(result.Value, "application/zip", "archive.zip");
    }

    [Authorization(Permissions.EncodeFiles | Permissions.DecodeFiles)]
    [HttpPost("decode")]
    public async Task<IActionResult> DecodeAsync([FromForm] List<IFormFile> files, [FromQuery] Guid userId, CancellationToken cancellationToken)
    {
        var command = new DecodeCommand
        (
            files.Select(x => new FileEntryDto(x.FileName, x.ContentType, x.Length, x.OpenReadStream())).ToList()
        );

        var result = await _dispatcher.SendAsync(command, cancellationToken);

        return File(result.Value, "application/zip", "archive.zip");
    }

    [Authorization(Permissions.DecodeFiles)]
    [HttpPost("decode/{fileName}")]
    public async Task<IActionResult> DecodeAsync(string fileName, CancellationToken cancellationToken)
    {
        var command = new DecodeByFileNameCommand
        (
            fileName
        );

        var result = await _dispatcher.SendAsync(command, cancellationToken);

        return File(result.Value, "application/zip", "archive.zip");
    }

    [Authorization(Permissions.ViewFiles)]
    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadFileAsync(string fileName)
    {
        var result = await _fileGetter.GetFileBytesAsync(fileName);

        return File(result.Value.Bytes, result.Value.ContentType, fileName);
    }

    [Authorization(Permissions.ViewFiles)]
    [HttpGet]
    public async Task<IActionResult> GetFilesAsync([FromQuery] GetFilesQuery query, CancellationToken cancellationToken)
        => Ok(await _dispatcher.QueryAsync(query, cancellationToken));
}
