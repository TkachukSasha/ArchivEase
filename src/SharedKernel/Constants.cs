namespace SharedKernel;

internal static class FileUnitsOfMeasurement
{
    internal static class Names
    {
        internal const string B = "b";

        internal const string KB = "kb";

        internal const string MB = "mb";

        internal const string GB = "gb";
    }

    internal static class Size
    {
        internal const int KB = 1024;

        internal const int MB = 1024 * KB;

        internal const int GB = 1024 * MB;
    }
}