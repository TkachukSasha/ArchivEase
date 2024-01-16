namespace SharedKernel.Types;

public static class Extensions
{
    public static bool ArrayOfBytesIsNullOrEmpty(this byte[]? bytes)
        => bytes is null || bytes.Length == 0 ? true : false;
}
