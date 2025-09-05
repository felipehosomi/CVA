using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapSkaWs.Logger
{
    public class Log
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Debug(string message)
        {
            _log.Debug(message);
        }

        public static void Debug(string message, Exception ex)
        {
            _log.Debug(message, ex);
        }

        public static void Error(string message)
        {
            _log.Error(message);
        }

        public static void Error(string message, Exception ex)
        {
            _log.Error(message, ex);
        }

        public static void Fatal(string message)
        {
            _log.Fatal(message);
        }

        public static void Fatal(string message, Exception ex)
        {
            _log.Fatal(message, ex);            
        }

        public static void Info(string message)
        {
            _log.Info(message);
        }

        public static void Info(string message, Exception ex)
        {
            _log.Info(message, ex);
        }

        public static void Warn(string message)
        {
            _log.Warn(message);
        }

        public static void Warn(string message, Exception ex)
        {
            _log.Warn(message, ex);
        }
    }
}
