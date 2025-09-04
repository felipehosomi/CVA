using System;
using System.Threading;
using System.Threading.Tasks;

namespace CVA_Rep_Logging
{
    public static class Extensions
    {
        public static void Raise<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            handler?.Invoke(sender, args);
        }

        public static Task HandleExceptions(this Task task, Action<Exception> exceptionsHandler)
        {
            return task.ContinueWith(t =>
            {
                var e = t.Exception;

                if (e == null)
                {
                    return;
                }

                e.Flatten().Handle(ie =>
                {
                    exceptionsHandler(ie);
                    return true;
                });
            },
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
        }
    }
}