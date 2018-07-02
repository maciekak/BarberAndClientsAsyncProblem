using System;
using System.Threading;

namespace BarberAndClientsAsyncTest
{
    public class ClientFactory
    {
        private readonly SimulationParameters _parameters;
        private readonly Random _generator = new Random();
        public ClientFactory()
        {
            _parameters = new SimulationParameters();
        }
        public ClientFactory(SimulationParameters parameters)
        {
            _parameters = parameters ?? new SimulationParameters();
        }

        public Client WaitForNextClient()
        {
            Thread.Sleep(_generator.Next(_parameters.MinTimeToNextClient, _parameters.MaxTimeToNextClient));
            return new Client(_generator.Next(_parameters.MinCutTime, _parameters.MaxCutTime));
        }
    }
}
