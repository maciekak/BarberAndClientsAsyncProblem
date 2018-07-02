using System;
using System.Linq;
using System.Threading.Tasks;

namespace BarberAndClientsAsyncTest
{
    public class HairdresserManager
    {
        private int _maxQueueSize = 20;
        private int _clientsPerDay = 100;
        private ClientFactory _clientFactory;
        private SyncQueue<Client> _queue;
        private Barber _barber;
        //TODO: use token instead this
        private bool _shouldStopWorking = false;

        public void Run()
        {
            _clientFactory = IfWantToPassParameters() ? new ClientFactory(LoadParameters()) : new ClientFactory();
            _queue = new SyncQueue<Client>(_maxQueueSize);
            _barber = new Barber(_queue, _clientsPerDay);

            var barberTask = Task.Factory.StartNew(_barber.Work);
            var clientsTask = Task.Factory.StartNew(SimulateQueue);

            barberTask.Wait();
            _shouldStopWorking = true;
            clientsTask.Wait();
        }

        private void SimulateQueue()
        {
            while (!_shouldStopWorking)
            {
                var wasQueueEmpty = _queue.IsEmpty();
                if (_queue.TryEnqueue(_clientFactory.WaitForNextClient()))
                {
                    Console.WriteLine("Next client waiting in queue");
                    if (wasQueueEmpty)
                        _barber.WakeUp();
                }
                else
                    Console.WriteLine("Next client has not fitted in queue");
            }
        }

        private bool IfWantToPassParameters()
        {
            Console.WriteLine("Do you want to pass parameters (y/n): ");
            return Console.ReadKey().KeyChar == 'y';
        }

        private SimulationParameters LoadParameters()
        {
            Console.WriteLine("Pass maximum queue size: ");
            _maxQueueSize = int.Parse(Console.ReadLine());

            Console.WriteLine("Pass how many clients per day: ");
            _clientsPerDay = int.Parse(Console.ReadLine());

            Console.WriteLine("Pass minimal cut time in ms: ");
            var minCutTime = int.Parse(Console.ReadLine());

            Console.WriteLine("Pass maximum cut time in ms: ");
            var maxCutTime = int.Parse(Console.ReadLine());

            Console.WriteLine("Pass minimal time to next client in ms: ");
            var minTimeToNextClient = int.Parse(Console.ReadLine());

            Console.WriteLine("Pass maximum time to next client in ms: ");
            var maxTimeToNextClient = int.Parse(Console.ReadLine());

            return new SimulationParameters(minCutTime, maxCutTime, minTimeToNextClient,
                maxTimeToNextClient);
        }
    }
}
