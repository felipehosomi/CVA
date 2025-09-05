using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace App.Domain.Helpers
{
    public enum LogSeverity
    {
        Debug,
        Info,
        Warning,
        Error
    }

    public delegate void LogEventDelegate(string logMessage);

    public class LogHelper
    {
        private const int LOG_QUEUE_LENGTH = 1000;

        static string logFile;
        static bool trace;
        static bool monochrome;
        static string traceFilter;
        static LogSeverity consoleLevel;
        static Queue<string> logQueue;

        public static event LogEventDelegate LogEvent;

        static LogHelper()
        {
            var path = Application.StartupPath + "\\_log\\";

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch
            {
                MessageBox.Show("Unable to create folder in path: " + Environment.NewLine + path + Environment.NewLine + "Please create this folder manually!");
                Environment.Exit(0);
            }
            

            logFile = Path.ChangeExtension(path + DateTime.Now.ToString("dd-MM-yyyy"), "log");
            consoleLevel = LogSeverity.Debug;
            logQueue = new Queue<string>();
        }

        public static bool Trace
        {
            get
            {
                return trace;
            }
            set
            {
                trace = value;
            }
        }

        public static string TraceFilter
        {
            get
            {
                return traceFilter;
            }
            set
            {
                traceFilter = value;
            }
        }

        public static bool Monochrome
        {
            get
            {
                return monochrome;
            }
            set
            {
                monochrome = value;
            }
        }

        public static LogSeverity ConsoleLevel
        {
            get
            {
                return consoleLevel;
            }
            set
            {
                consoleLevel = value;
            }
        }

        public static void Debug(string message, params object[] pars)
        {
            LogWrite(message, LogSeverity.Debug, pars);
        }
        public static void Info(string message, params object[] pars)
        {
            LogWrite(message, LogSeverity.Info, pars);
        }
        public static void Warning(string message, params object[] pars)
        {
            LogWrite(message, LogSeverity.Warning, pars);
        }
        public static void Dump(string message, params object[] pars)
        {
            LogWrite(message, LogSeverity.Info, pars);
        }
        public static void Error(Exception ex, string message, params object[] pars)
        {
            LogWrite(string.Format(message, pars), LogSeverity.Error, ex);
        }

        private static void LogWrite(string str, LogSeverity severity, params object[] pars)
        {
            LogWrite(string.Format(str, pars), severity);
        }

        private static void LogWrite(string str, LogSeverity severity, Exception ex = (Exception)null)
        {
            StackFrame stackFrame = null;

            var stackTrace = new StackTrace();

            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                stackFrame = stackTrace.GetFrame(i);
                if (stackFrame == null)
                    break;

                if (stackFrame.GetMethod().DeclaringType.Name != "Log")
                    break;
            }

            if (stackFrame == null)
                LogWrite(str, severity, ex, (MethodBase)null);
            else
            {
                MethodBase m = stackFrame.GetMethod();
                LogWrite(str, severity, ex, m);
            }
        }

        private static void LogWrite(string str, LogSeverity severity, Exception ex, MethodBase callingMethod)
        {
            try
            {
                if (severity >= consoleLevel)
                {
                    if (!monochrome)
                    {
                        switch (severity)
                        {
                            case LogSeverity.Info:
                                Console.ResetColor();
                                break;
                            case LogSeverity.Warning:
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                break;
                            case LogSeverity.Error:
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;
                            case LogSeverity.Debug:
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                break;
                        }
                    }
                    Console.WriteLine(string.Format("<{0}> {1}", severity.ToString(), str));
                    Console.ResetColor();
                }

                string s = null;

                //Save to log file
                try
                {
                    string caller = "Unknown";
                    if (callingMethod != null)
                        if (callingMethod.DeclaringType != null)
                            caller = string.Format("{0}.{1}", callingMethod.DeclaringType.Name, callingMethod.Name).PadRight(45, ' ');

                    string exMessage = (ex != null) ? "\t" + ex.Message : null;
                    exMessage += (ex != null && ex.InnerException != null) ? "\r InnerException: " + ex.InnerException.Message : "";

                    s = string.Format("{0:yyyy-MM-dd HH:mm:ss.ffff}    {1} {3}{4}{5}\t{2}", DateTime.Now, string.IsNullOrEmpty(Thread.CurrentThread.Name) ? "UnamedThread".PadRight(30, ' ') : Thread.CurrentThread.Name.PadRight(30, ' '), caller, str, exMessage, (ex != null) ? "|" + ex.StackTrace : null);

                    s = s.Replace("\r", "|").Replace("\n", "|");

                    lock (logFile)
                    {
                        using (TextWriter tw = new StreamWriter(logFile, true))
                        {
                            lock (tw)
                            {
                                tw.WriteLine(s);
                            }
                        }
                    }
                }
                catch
                {
                }

                //Fire log event
                try
                {
                    if (LogEvent != null)
                        LogEvent(s);
                }
                catch
                {
                }

                //Populate LogQueue
                try
                {
                    lock (logQueue)
                    {
                        logQueue.Enqueue(s);
                        if (logQueue.Count > LOG_QUEUE_LENGTH)
                            logQueue.Dequeue();
                    }
                }
                catch
                {
                }
            }
            catch (Exception myEx)
            {
                Console.WriteLine("Error trying to write log message [{0}]. Error: {1}", str, myEx.Message);
            }
        }

        public static void WriteTrace()
        {
            if (!trace)
                return;

            MethodBase m = new StackTrace().GetFrame(1).GetMethod();
            if (string.IsNullOrEmpty(traceFilter) || string.Compare(m.DeclaringType.Name, traceFilter, StringComparison.OrdinalIgnoreCase) == 0)
            {
                Console.WriteLine(string.Format("Thread: {0} - {1}.{2}", System.Threading.Thread.CurrentThread.Name, m.DeclaringType.Name, m.Name));
            }
        }

        public static string GetLogSegment(DateTime start, DateTime end)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                TextReader tr = new StreamReader(logFile);
                string line;
                DateTime time;
                while (!string.IsNullOrEmpty(line = tr.ReadLine()))
                {
                    time = DateTime.ParseExact(line.Split('\t')[0], "yyyy-MM-dd HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture);
                    if (time >= start && time <= end)
                        sb.AppendLine(line);
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error trying to read log segment. Error: {0}", ex.Message);
                LogWrite(msg, LogSeverity.Error, ex);
                return msg;
            }
        }

        public static string ObjectToString(object o)
        {
            try
            {
                return new JavaScriptSerializer().Serialize(o);
            }
            catch
            {
                return o.ToString();
            }
        }

        public static string[] GetLiveLog()
        {
            return logQueue.ToArray();
        }
    }
}