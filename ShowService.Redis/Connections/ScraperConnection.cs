using System.Net.Http;
using System.Threading.Tasks;
using System;
using Common.Types.Abstractions;
using Polly;
using Common.Types.Network;

namespace ShowService.Redis.Connections
{
    public class ScraperConnection : IScraperConnection
    {
        private IHttpHandler _client;
        private HttpConfiguration _httpConfiguration;
        private NetworkConfiguration _networkConfiguration;

        public ScraperConnection(IHttpHandler client, HttpConfiguration httpConfiguration, NetworkConfiguration networkConfiguration)
        {
            _networkConfiguration = networkConfiguration;
            _httpConfiguration = httpConfiguration;
            _client = client;
        }

        public async Task<int> ScrapAsync(int apiOffset, int apiCount)
        {
            var url = GetScraperUrl(apiOffset, apiCount);
            var result = await GetAsync(url);
            result.EnsureSuccessStatusCode();
            return int.Parse(await result.Content.ReadAsStringAsync());
        }

        private async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await Policy.HandleResult<HttpResponseMessage>(r => r.StatusCode == System.Net.HttpStatusCode.NotFound)
                               .WaitAndRetryAsync(_httpConfiguration.HttpRetryTimes, i => TimeSpan.FromSeconds(Math.Pow(2, i)))
                               .ExecuteAsync(() => _client.GetAsync(url));
        }

        private string GetScraperUrl(int apiOffset, int apiCount)
        {
            string baseUrl = _networkConfiguration.ScraperEndpoint.Root + _networkConfiguration.ScraperEndpoint.Shows;
            return string.Format(baseUrl, apiOffset, apiCount);
        }
    }
}
