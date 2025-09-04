using System;

namespace CVA_Rep_Logging
{
    public interface ILogger
    {
        void Debug(object message);
        void Debug(object message, Exception ex);
        void DebugFormat(string format, object arg);
        void DebugFormat(string format, object arg1, object arg2);
        void DebugFormat(string format, object arg1, object arg2, object arg3);
        void DebugFormat(string format, params object[] args);
        void DebugFormat(IFormatProvider provider, string format, params object[] args);

        void Info(object message);
        void Info(object message, Exception ex);
        void InfoFormat(string format, object arg1, object arg2);
        void InfoFormat(string format, object arg1, object arg2, object arg3);
        void InfoFormat(string format, params object[] args);
        void InfoFormat(IFormatProvider provider, string format, params object[] args);

        void Warn(object message);
        void Warn(object message, Exception ex);
        void WarnFormat(string format, object arg1, object arg2);
        void WarnFormat(string format, object arg1, object arg2, object arg3);
        void WarnFormat(string format, params object[] args);
        void WarnFormat(IFormatProvider provider, string format, params object[] args);

        void Error(object message);
        void Error(object message, Exception ex);
        void ErrorFormat(string format, object arg1, object arg2);
        void ErrorFormat(string format, object arg1, object arg2, object arg3);
        void ErrorFormat(string format, params object[] args);
        void ErrorFormat(IFormatProvider provider, string format, params object[] args);

        void Fatal(object message);
        void Fatal(object message, Exception ex);
        void FatalFormat(string format, object arg1, object arg2);
        void FatalFormat(string format, object arg1, object arg2, object arg3);
        void FatalFormat(string format, params object[] args);
        void FatalFormat(IFormatProvider provider, string format, params object[] args);
    }
}