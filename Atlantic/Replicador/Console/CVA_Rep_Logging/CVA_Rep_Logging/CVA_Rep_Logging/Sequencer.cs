using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CVA_Rep_Logging
{
    public sealed class Sequencer<T>
    {
        private readonly BlockingCollection<T> _queue;
        private Task _worker;

        public Sequencer(Action<T> consumer)
            : this(-1, consumer)
        {
        }

        public Sequencer(Action<T> consumer, int boundedCapacity)
            : this(boundedCapacity, consumer)
        {
            if (boundedCapacity <= 0)
            {
                throw new ArgumentException("Bounded capacity should be greater than zero.");
            }
        }

        private Sequencer(int boundedCapacity, Action<T> consumer)
        {
            if (consumer == null)
            {
                throw new ArgumentNullException(nameof(consumer));
            }

            _queue = boundedCapacity > 0 ? new BlockingCollection<T>(boundedCapacity) : new BlockingCollection<T>();
            SetupConsumer(consumer, 1);
        }

        public int Capacity => _queue.BoundedCapacity;
        public uint PendingCount => (uint) _queue.Count;
        public T[] PendingItems => _queue.ToArray();
        public bool ShutdownRequested => _queue.IsAddingCompleted;
        public event EventHandler<SequencerExceptionEventArgs> OnException;

        public void Enqueue(T item)
        {
            _queue.Add(item);
        }

        public bool TryEnqueue(T item)
        {
            return _queue.TryAdd(item);
        }

        public bool TryEnqueue(T item, TimeSpan timeout)
        {
            return _queue.TryAdd(item, timeout);
        }

        public void Shutdown(bool waitForPendingItems = true)
        {
            _queue.CompleteAdding();
            if (waitForPendingItems)
            {
                _worker.Wait();
            }
        }

        private void SetupConsumer(Action<T> consumer, int workerCount)
        {
            Action work = () =>
            {
                foreach (var item in _queue.GetConsumingEnumerable())
                {
                    consumer(item);
                }
            };

            for (var i = 0; i < workerCount; i++)
            {
                var task = new Task(work, TaskCreationOptions.LongRunning);
                task.HandleExceptions(
                    e =>
                    {
                        OnException.Raise(this,
                            new SequencerExceptionEventArgs(new SequencerException("Exception occurred.", e)));
                    });

                _worker = task;
                _worker.Start();
            }
        }
    }
}