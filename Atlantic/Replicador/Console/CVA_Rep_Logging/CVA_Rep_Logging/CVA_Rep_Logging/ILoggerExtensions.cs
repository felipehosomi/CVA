namespace CVA_Rep_Logging
{
    public static class ILoggerExtensions
    {
        public static ScopedLogger GetScopedLogger(this ILogger logger, string name, LogLevel level)
        {
            return new ScopedLogger(logger, name, level);
        }
    }
}