using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Halloumi.Common.Helpers
{
    public static class ParallelHelper
    {
        /// <summary>
        /// Enumerates through each item in a list in parallel
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="list">The list of items.</param>
        /// <param name="action">The action to perform.</param>
        public static void ForEach<T>(IEnumerable<T> list, Action<T> action)
        {
            var chunkedLists = GetChunkedLists<T>(list.ToList());

            foreach (var chunkedList in chunkedLists)
            {
                using (var countdown = new Countdown(chunkedList.Count()))
                {
                    foreach (var current in chunkedList)
                    {
                        var captured = current;
                        ThreadPool.QueueUserWorkItem(x =>
                        {
                            action.Invoke(captured);
                            countdown.Signal();
                        });
                    }
                    countdown.Wait();
                }
            }
        }

        private static IEnumerable<List<T>> GetChunkedLists<T>(List<T> list)
        {
            var chunkSize = Environment.ProcessorCount * 16;
            var itemsReturned = 0;
            var count = list.Count;
            while (itemsReturned < count)
            {
                var currentChunkSize = Math.Min(chunkSize, count - itemsReturned);
                yield return list.GetRange(itemsReturned, currentChunkSize);
                itemsReturned += currentChunkSize;
            }
        }

        /// <summary>
        /// Countdown class
        /// </summary>
        private class Countdown : IDisposable
        {
            private readonly ManualResetEvent _doneEvent;
            private volatile int _currentIndex;

            /// <summary>
            /// Initializes a new instance of the <see cref="Countdown"/> class.
            /// </summary>
            /// <param name="total">The total.</param>
            public Countdown(int total)
            {
                _currentIndex = total;
                _doneEvent = new ManualResetEvent(false);
            }

            /// <summary>
            /// Signals this instance.
            /// </summary>
            public void Signal()
            {
                lock (_doneEvent)
                {
                    if (_currentIndex > 0 && --_currentIndex == 0)
                        _doneEvent.Set();
                }
            }

            /// <summary>
            /// Waits this instance.
            /// </summary>
            public void Wait()
            {
                _doneEvent.WaitOne();
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                ((IDisposable)_doneEvent).Dispose();
            }
        }
    }
}