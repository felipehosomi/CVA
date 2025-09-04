using System;
using System.Threading;
using System.Timers;
using log4net.Appender;
using log4net.Core;
using log4net.Util;
using Timer = System.Timers.Timer;

namespace CVA_Rep_Logging
{
    public class AsyncBufferingForwardingAppender : BufferingForwardingAppender
    {
        private readonly Timer _idleFlushTimer;
        private readonly TimeSpan _idleTimeThreshold;
        private readonly Sequencer<Action> _sequencer;
        private DateTime _lastFlushTime;

        public AsyncBufferingForwardingAppender()
        {
            _sequencer = new Sequencer<Action>(action => action(), 1);
            _sequencer.OnException += OnSequencerException;

            _idleTimeThreshold = TimeSpan.FromMilliseconds(500);
            _idleFlushTimer = new Timer(_idleTimeThreshold.TotalSeconds*1000);
            _idleFlushTimer.Elapsed += InvokeFlushIfIdle;
            _idleFlushTimer.Start();
        }

        private bool IsIdle
        {
            get
            {
                if (DateTime.UtcNow - _lastFlushTime >= _idleTimeThreshold)
                {
                    return true;
                }
                return false;
            }
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            LogWarningIfLossy();
        }

        protected override void SendBuffer(LoggingEvent[] events)
        {
            if (!_sequencer.ShutdownRequested)
            {
                _sequencer.Enqueue(() => base.SendBuffer(events));
            }
            else
            {
                base.SendBuffer(events);
            }

            _lastFlushTime = DateTime.UtcNow;
        }

        protected override void OnClose()
        {
            _idleFlushTimer.Elapsed -= InvokeFlushIfIdle;
            _idleFlushTimer.Dispose();

            _sequencer.Shutdown();
            _sequencer.OnException -= OnSequencerException;

            base.OnClose();
        }

        private void InvokeFlushIfIdle(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (!IsIdle)
            {
                return;
            }
            Flush();
        }

        private void LogWarningIfLossy()
        {
            if (!Lossy)
            {
                return;
            }

            var warning = new LoggingEvent(new LoggingEventData
            {
                Level = Level.Warn,
                LoggerName = GetType().Name,
                ThreadName = Thread.CurrentThread.ManagedThreadId.ToString(),
                TimeStamp = DateTime.UtcNow,
                Message = "This is a 'lossy' appender therefore log messages may be dropped."
            });

            Lossy = false;
            Append(warning);
            Flush();
            Lossy = true;
        }

        private void OnSequencerException(object sender, SequencerExceptionEventArgs args)
        {
            LogLog.Error(GetType(), "An exception occurred while processing LogEvents.", args.Exception);
        }
    }
}