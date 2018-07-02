using System.Collections.Generic;

namespace BarberAndClientsAsyncTest
{
    //I know that there is ConcurrateQueue, but i wanted to implemented it by myself
    class SyncQueue <T>
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
                    return true;
                }

                item = default(T);
                return false;
            }
        }
    }
}
