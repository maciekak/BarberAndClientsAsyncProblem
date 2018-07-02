namespace BarberAndClientsAsyncTest
{
    public class SimulationParameters
    {
        public int MinCutTime { get; }
        public int MaxCutTime { get; }
        public int MinTimeToNextClient { get; }
        public int MaxTimeToNextClient { get; }

        public SimulationParameters(int minCutTime = 500, int maxCutTime = 3000,
            int minTimeToNextClient = 100, int maxTimeToNextClient = 5000)
        {
            MinCutTime = minCutTime;
            MaxCutTime = maxCutTime;
            MinTimeToNextClient = minTimeToNextClient;
            MaxTimeToNextClient = maxTimeToNextClient;
        }

    }
}
