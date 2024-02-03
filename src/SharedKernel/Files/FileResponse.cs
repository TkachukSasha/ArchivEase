namespace SharedKernel.Files;

public sealed class FileResponse
{
    public byte[] Bytes { get; set; } = [];
    public string ContentType { get; set; } = string.Empty;
}
