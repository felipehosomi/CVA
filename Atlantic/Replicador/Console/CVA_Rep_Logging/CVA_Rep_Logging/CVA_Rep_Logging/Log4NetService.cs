using System;
using System.Diagnostics;
using System.IO;
using log4net;
using log4net.Config;

namespace CVA_Rep_Logging
{
    public sealed class Log4NetService : ILogService
    {
        private static readonly Lazy<Log4NetService> Lazy = new Lazy<Log4NetService>(() => new Log4NetService(), true);

        private Log4NetService()
        {
            var log4NetConfigDir = AppDomain.CurrentDomain.RelativeSearchPath;

            if (string.IsNullOrWhiteSpace(log4NetConfigDir))
            {
                log4NetConfigDir = AppDomain.CurrentDomain.BaseDirectory;
            }

            var defaultConfigFile = new FileInfo(Path.Combine(log4NetConfigDir, "log4net.config"));
            ConfigureImpl(defaultConfigFile);
        }

        public static Log4NetService Instance => Lazy.Value;

        [DebuggerStepThrough]
        public void Dispose()
        {
            LogManager.Shutdown();
        }

        public void Configure(FileInfo configFile)
        {
            ConfigureImpl(configFile);
        }

        [DebuggerStepThrough]
        public ILogger GetLogger(string loggerName)
        {
            return new Log4NetLogger(LogManager.GetLogger(loggerName));
        }

        [DebuggerStepThrough]
        public ILogger GetLogger(Type loggerType)
        {
            return new Log4NetLogger(LogManager.GetLogger(loggerType));
        }

        [DebuggerStepThrough]
        public ILogger GetLogger<T>()
        {
            return GetLogger(typeof (T));
        }

        private static void ConfigureImpl(FileInfo configFile)
        {
            if (configFile == null)
            {
                throw new ArgumentException("Config file cannot be null", nameof(configFile));
            }
            if (!configFile.Exists)
            {
                throw new FileNotFoundException("Could not find a valid log4net configuration file", configFile.FullName);
            }

            XmlConfigurator.ConfigureAndWatch(configFile);
        }
    }
}