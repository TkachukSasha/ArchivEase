namespace SharedKernel.Files;

public static class FileExtensions
{
    public static (double, string) GetFileSizeUnitsOfMeasurement(long length)
    {
        if (length == 0) return (0.0d, FileUnitsOfMeasurement.Names.B);

        string[] sizes = [
            FileUnitsOfMeasurement.Names.B,
            FileUnitsOfMeasurement.Names.KB,
            FileUnitsOfMeasurement.Names.MB,
            FileUnitsOfMeasurement.Names.GB
        ];

        int i = (int)Math.Floor(Math.Log(length) / Math.Log(1024));

        return (length / Math.Pow(1024, i), sizes[i]);
    }

    public static string GetContentType(string fileExtension) =>
        fileExtension.ToLower() switch
        {
            ".txt" => "text/plain",
            ".pdf" => "application/pdf",
            ".png" => "image/png",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".ogg" => "audio/ogg",
            ".flac" => "audio/flac",
            _ => "application/octet-stream"
        };
}
