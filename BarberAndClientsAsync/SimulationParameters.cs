namespace BarberAndClientsAsyncTest
{
    public class SimulationParameters
    {
        public int QueueSize { get; }
        public int ClientsPerDay { get; }
        public int MinCutTime { get; }
        public int MaxCutTime { get; }
        public int MinTimeToNextClient { get; }
        public int MaxTimeToNextClient { get; }

        public SimulationParameters(int clientsPerDay = 100, int queueSize = 40, int minCutTime = 500, int maxCutTime = 3000,
            int minTimeToNextClient = 100, int maxTimeToNextClient = 5000)
        {
            ClientsPerDay = clientsPerDay;
            QueueSize = queueSize;
            MinCutTime = minCutTime;
            MaxCutTime = maxCutTime;
            MinTimeToNextClient = minTimeToNextClient;
            MaxTimeToNextClient = maxTimeToNextClient;
        }

    }
}
