using Common.Types.Abstractions;
using Polly;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Types.Network
{
    public class HttpClientHandler : IHttpHandler
    {
        private HttpClient _client = new HttpClient();
        private HttpConfiguration _config;

        public HttpClientHandler(HttpConfiguration config)
        {
            _config = config;
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return Policy.HandleResult<HttpResponseMessage>(r => r.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                         .OrResult(x => x.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                         .OrResult(x => x.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                         .OrResult(x => x.StatusCode == System.Net.HttpStatusCode.BadGateway)
                         .CircuitBreakerAsync(_config.CircuitBreakerLimit, TimeSpan.FromMinutes(5))
                         .ExecuteAsync(() => _client.GetAsync(url));
        }
    }
}
