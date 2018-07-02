namespace BarberAndClientsAsyncTest
{
    public class SimulationParameters
    {
        public int MinCutTime { get; }
        public int MaxCutTime { get; }
        public int MinTimeToNextClient { get; }
        public int MaxTimeToNextClient { get; }

        public SimulationParameters(int minCutTime = 50, int maxCutTime = 300,
            int minTimeToNextClient = 10, int maxTimeToNextClient = 500)
        {
            MinCutTime = minCutTime;
            MaxCutTime = maxCutTime;
            MinTimeToNextClient = minTimeToNextClient;
            MaxTimeToNextClient = maxTimeToNextClient;
        }

    }
}
