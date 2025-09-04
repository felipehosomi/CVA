using System;
using System.Diagnostics;
using log4net;

namespace CVA_Rep_Logging
{
    internal sealed class Log4NetLogger : ILogger
    {
        private readonly ILog _logger;

        [DebuggerStepThrough]
        internal Log4NetLogger(ILog logger)
        {
            _logger = logger;
        }

        public bool IsDebugEnabled => _logger.IsDebugEnabled;
        public bool IsInfoEnabled => _logger.IsInfoEnabled;
        public bool IsWarnEnabled => _logger.IsWarnEnabled;
        public bool IsErrorEnabled => _logger.IsErrorEnabled;
        public bool IsFatalEnabled => _logger.IsFatalEnabled;

        #region Debug Methods

        [DebuggerStepThrough]
        public void Debug(object message)
        {
            _logger.Debug(message);
        }

        [DebuggerStepThrough]
        public void Debug(object message, Exception ex)
        {
            _logger.Debug(message, ex);
        }

        [DebuggerStepThrough]
        public void DebugFormat(string format, object arg)
        {
            _logger.DebugFormat(format, arg);
        }

        [DebuggerStepThrough]
        public void DebugFormat(string format, object arg1, object arg2)
        {
            _logger.DebugFormat(format, arg1, arg2);
        }

        [DebuggerStepThrough]
        public void DebugFormat(string format, object arg1, object arg2, object arg3)
        {
            _logger.DebugFormat(format, arg1, arg2, arg3);
        }

        [DebuggerStepThrough]
        public void DebugFormat(string format, params object[] args)
        {
            _logger.DebugFormat(format, args);
        }

        [DebuggerStepThrough]
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.DebugFormat(provider, format, args);
        }

        #endregion

        #region Info Methods

        [DebuggerStepThrough]
        public void Info(object message)
        {
            _logger.Info(message);
        }

        [DebuggerStepThrough]
        public void Info(object message, Exception ex)
        {
            _logger.Info(message, ex);
        }

        [DebuggerStepThrough]
        public void InfoFormat(string format, object arg)
        {
            _logger.InfoFormat(format, arg);
        }

        [DebuggerStepThrough]
        public void InfoFormat(string format, object arg1, object arg2)
        {
            _logger.InfoFormat(format, arg1, arg2);
        }

        [DebuggerStepThrough]
        public void InfoFormat(string format, object arg1, object arg2, object arg3)
        {
            _logger.InfoFormat(format, arg1, arg2, arg3);
        }

        [DebuggerStepThrough]
        public void InfoFormat(string format, params object[] args)
        {
            _logger.InfoFormat(format, args);
        }

        [DebuggerStepThrough]
        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.InfoFormat(provider, format, args);
        }

        #endregion

        #region Warning Methods

        [DebuggerStepThrough]
        public void Warn(object message)
        {
            _logger.Warn(message);
        }

        [DebuggerStepThrough]
        public void Warn(object message, Exception ex)
        {
            _logger.Warn(message, ex);
        }

        [DebuggerStepThrough]
        public void WarnFormat(string format, object arg)
        {
            _logger.WarnFormat(format, arg);
        }

        [DebuggerStepThrough]
        public void WarnFormat(string format, object arg1, object arg2)
        {
            _logger.WarnFormat(format, arg1, arg2);
        }

        [DebuggerStepThrough]
        public void WarnFormat(string format, object arg1, object arg2, object arg3)
        {
            _logger.WarnFormat(format, arg1, arg2, arg3);
        }

        [DebuggerStepThrough]
        public void WarnFormat(string format, params object[] args)
        {
            _logger.WarnFormat(format, args);
        }

        [DebuggerStepThrough]
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.WarnFormat(provider, format, args);
        }

        #endregion

        #region Error Methods

        [DebuggerStepThrough]
        public void Error(object message)
        {
            _logger.Error(message);
        }

        [DebuggerStepThrough]
        public void Error(object message, Exception ex)
        {
            _logger.Error(message, ex);
        }

        [DebuggerStepThrough]
        public void ErrorFormat(string format, object arg)
        {
            _logger.ErrorFormat(format, arg);
        }

        [DebuggerStepThrough]
        public void ErrorFormat(string format, object arg1, object arg2)
        {
            _logger.ErrorFormat(format, arg1, arg2);
        }

        [DebuggerStepThrough]
        public void ErrorFormat(string format, object arg1, object arg2, object arg3)
        {
            _logger.ErrorFormat(format, arg1, arg2, arg3);
        }

        [DebuggerStepThrough]
        public void ErrorFormat(string format, params object[] args)
        {
            _logger.ErrorFormat(format, args);
        }

        [DebuggerStepThrough]
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.ErrorFormat(provider, format, args);
        }

        #endregion

        #region Fatal Methods

        [DebuggerStepThrough]
        public void Fatal(object message)
        {
            _logger.Fatal(message);
        }

        [DebuggerStepThrough]
        public void Fatal(object message, Exception ex)
        {
            _logger.Fatal(message, ex);
        }

        [DebuggerStepThrough]
        public void FatalFormat(string format, object arg)
        {
            _logger.FatalFormat(format, arg);
        }

        [DebuggerStepThrough]
        public void FatalFormat(string format, object arg1, object arg2)
        {
            _logger.FatalFormat(format, arg1, arg2);
        }

        [DebuggerStepThrough]
        public void FatalFormat(string format, object arg1, object arg2, object arg3)
        {
            _logger.FatalFormat(format, arg1, arg2, arg3);
        }

        [DebuggerStepThrough]
        public void FatalFormat(string format, params object[] args)
        {
            _logger.FatalFormat(format, args);
        }

        [DebuggerStepThrough]
        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.FatalFormat(provider, format, args);
        }

        #endregion
    }
}