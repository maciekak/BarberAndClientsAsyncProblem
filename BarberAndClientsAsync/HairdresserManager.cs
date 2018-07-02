using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BarberAndClientsAsyncTest
{
    public class HairdresserManager
    {
        private int _maxQueueSize = 5;
        private int _clientsPerDay = 10;
        private ClientFactory _clientFactory;
        private SyncQueue<Client> _queue;
        private Barber _barber;
        private CancellationToken _cancellationToken;

        public void Run()
        {
            _clientFactory = IfWantToPassParameters() ? new ClientFactory(LoadParameters()) : new ClientFactory();
            _queue = new SyncQueue<Client>(_maxQueueSize);
            _barber = new Barber(_queue, _clientsPerDay);

            using (var clientsTokenSource = new CancellationTokenSource())
            {
                var barberTask = Task.Factory.StartNew(_barber.Work);
                _cancellationToken = clientsTokenSource.Token;
                var clientsTask = Task.Factory.StartNew(SimulateQueue, clientsTokenSource.Token);

                barberTask.Wait();
                clientsTokenSource.Cancel();
                clientsTask.Wait();
            }

            Console.WriteLine("End of the day");
            Console.ReadKey();
        }

        private void SimulateQueue()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                if (_queue.TryEnqueue(_clientFactory.WaitForNextClient()))
                {
                    Console.WriteLine("Next client waiting in queue");
                    if (_queue.IsOnlyOneElement())
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
