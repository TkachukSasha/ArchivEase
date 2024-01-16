namespace SharedKernel.Authentication.Internal;

internal static class Extensions
{
    internal static long ToTimestamp(this DateTimeOffset date)
    {
        var centuryBegin = DateTimeOffset.UnixEpoch;

        var expectedDate = date.Subtract(new TimeSpan(centuryBegin.Ticks));

        return expectedDate.Ticks / 10000;
    }
}