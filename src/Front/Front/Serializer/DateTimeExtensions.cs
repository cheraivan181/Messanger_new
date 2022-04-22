namespace Core.BinarySerializer;

public static class DateTimeExtensions
{
    private static readonly DateTime StartDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Convert date and time to Unix time stamp
    /// </summary>
    /// <param name="time">Custom date and time</param>
    /// <returns>Seconds from 1.1.1970</returns>
    public static int ToUnixTime(this DateTime time)
    {
        return (int) time.Subtract(StartDate).TotalSeconds;
    }

    /// <summary>
    /// Get date and time from 1.01.1970
    /// </summary>
    /// <param name="seconds">Unix time stamp</param>
    /// <returns></returns>
    public static DateTime ToDateTime(this int seconds)
    {
        return StartDate.AddSeconds(seconds);
    }
}