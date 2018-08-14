namespace Common.Types.Network
{
    public class NetworkConfiguration
    {
        public string RedisConnection { get; set; }
        public ScraperEndpoint ScraperEndpoint { get; set; }
        public int ScraperPageLimit { get; set; }
    }
}
