using System;
using System.Collections.Generic;

namespace CommonExtensions
{
    public static class QueueExtension
    {
        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            if (queue is null) throw new ArgumentNullException(nameof(queue));

            foreach (T item in items)
            {
                queue.Enqueue(item);
            }
        }
    }
}
