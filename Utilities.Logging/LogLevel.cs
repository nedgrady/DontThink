namespace Utilities.Logging
{
    public enum LogLevel
    {
        Verbose,
        Information,
        Warning,
        Error,
        Critical
    }

    static class LogLevelExtentions
    {
        public static string ToUserFriendlyName(this LogLevel level) => level.ToString();
    }
}
