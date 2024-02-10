namespace SharedKernel;

internal static class FileUnitsOfMeasurement
{
    internal static class Names
    {
        internal const string B = "B";

        internal const string KB = "KB";

        internal const string MB = "MB";

        internal const string GB = "GB";
    }

    internal static class Size
    {
        internal const int KB = 1024;

        internal const int MB = 1024 * KB;

        internal const int GB = 1024 * MB;
    }
}