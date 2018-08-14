namespace Common.Types.Network
{
    public class HttpConfiguration
    {
        public int HttpRetryTimes { get; set; }
        public int CircuitBreakerLimit { get; set; }
    }
}
