using System;
using System.Collections.Generic;

namespace BarberAndClientsAsyncTest
{
    //I know that there is ConcurrateQueue, but i wanted to implemented it by myself
    public class SyncQueue <T>
    {
        private readonly int _maxSize;
        private readonly Queue<T> _queue;
        private readonly object _queueMonitor = new object();
        private int _size;
        public SyncQueue(int size)
        {
            _maxSize = size;
            _size = 0;
            _queue = new Queue<T>(_maxSize);
        }

        public bool TryEnqueue(T item)
        {
            lock (_queueMonitor)
            {
                if (_size < _maxSize)
                {
                    _queue.Enqueue(item);
                    _size++;
                    Console.WriteLine("After enqueue, clients in queue: {0}", _size);
                    return true;
                }

                return false;
            }
        }

        public bool TryDequeue(out T item)
        {
            lock (_queueMonitor)
            {
                if (_size > 0)
                {
                    item = _queue.Dequeue();
                    _size--;
                    Console.WriteLine("After dqeueue, clients in queue: {0}", _size);
                    return true;
                }

                item = default(T);
                return false;
            }
        }

        public bool IsEmpty()
        {
            lock (_queueMonitor)
            {
                return _size == 0;
            }
        }

        public bool IsOnlyOneElement()
        {
            lock (_queueMonitor)
            {
                return _size == 1;
            }
        }
    }
}
