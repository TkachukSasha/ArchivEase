using Microsoft.AspNetCore.Authorization;

namespace SharedKernel.Authentication.Internal;

internal class FileAuthorizationRequirement : IAuthorizationRequirement
{
    public FileAuthorizationRequirement(int maxFilesForAnonymous) 
        => MaxFilesForAnonymous = maxFilesForAnonymous;

    public int MaxFilesForAnonymous { get; }
}