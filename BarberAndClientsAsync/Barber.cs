using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BarberAndClientsAsyncTest
{
    public class Barber
    {
        private readonly SyncQueue<Client> _queue;
        private readonly int _clientsPerDay;
        private readonly object _stateMonitor = new object();

        private BarberState _state = BarberState.Waiting;
        private int _handledClientsCount = 0;


        public Barber(SyncQueue<Client> queue, int clientsPerDay)
        {
            _queue = queue;
            _clientsPerDay = clientsPerDay;
        }

        public void Work()
        {
            while (_handledClientsCount < _clientsPerDay)
            {
                bool possibleToCutClient = false;
                lock (_stateMonitor)
                {
                    if (_state == BarberState.Sleeping)
                        continue;

                    possibleToCutClient = IfPossibleToCutClient();

                    if (!possibleToCutClient && _state == BarberState.Waiting)
                    {
                        _state = BarberState.Sleeping;
                        Console.WriteLine("Barber went to sleep");
                    }
                }

                if (possibleToCutClient)
                    CutClient();
            }
        }

        public bool WakeUp()
        {
            lock (_stateMonitor)
            {
                if (_state != BarberState.Sleeping)
                    return false;

                Console.WriteLine("Woke up barber");
                _state = BarberState.Waiting;
                return true;
            }
        }

        private bool IfPossibleToCutClient()
        {
            if (_state != BarberState.Waiting || _queue.IsEmpty())
                return false;
            
            return true;
        }
        private void CutClient()
        {
            _queue.TryDequeue(out var client);

            lock (_stateMonitor)
            {
                _state = BarberState.Cutting;
            }
            Console.WriteLine("Starting cutting client: {0}", _handledClientsCount + 1);

            Thread.Sleep(client.CutTime);

            Console.WriteLine("Cutting done");

            lock (_stateMonitor)
            {
                _state = BarberState.Waiting;
            }

            _handledClientsCount++;
        }
    }
}
