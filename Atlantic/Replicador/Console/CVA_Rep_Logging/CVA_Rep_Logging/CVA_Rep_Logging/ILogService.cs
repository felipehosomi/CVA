using System;
using System.IO;

namespace CVA_Rep_Logging
{
    public interface ILogService : IDisposable
    {
        void Configure(FileInfo configFile);
        ILogger GetLogger(string loggerName);
        ILogger GetLogger(Type loggerType);
        ILogger GetLogger<T>();
    }
}