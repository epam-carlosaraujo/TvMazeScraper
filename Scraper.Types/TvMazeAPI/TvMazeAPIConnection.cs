using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Types.Abstractions;
using Newtonsoft.Json;
using Scraper.Types.Abstractions;
using Scraper.Types.Exceptions;

namespace Scraper.Types.TvMazeAPI
{
    public class TvMazeAPIConnection : ITvMazeAPIConnection
    {
        private TvMazeAPIConfiguration _config;
        private IHttpHandler _client;

        public int ShowsPageLimit => _config.ShowsPageLimit;

        public TvMazeAPIConnection(IHttpHandler client, TvMazeAPIConfiguration configuration)
        {
            _config = configuration;
            _client = client;
        }

        public async Task<IEnumerable<TvMazeAPITypes.Show>> GetShowsAsync(int page)
        {
            var endpoints = _config.Endpoints;
            return await Get<IEnumerable<TvMazeAPITypes.Show>>(endpoints.ForShows(page));
        }

        public async Task<IEnumerable<TvMazeAPITypes.Cast>> GetCastByShowIdAsync(int showId)
        {
            var endpoints = _config.Endpoints;
            return await Get<IEnumerable<TvMazeAPITypes.Cast>>(endpoints.ForCast(showId));
        }

        private async Task<T> Get<T>(string endpoint) where T : class
        {
            HttpResponseMessage result = null;
            try
            {
                result = await _client.GetAsync(endpoint);
                result.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
            }
            catch(HttpRequestException) when ((int)result?.StatusCode == 429)
            {
                throw new TooManyRequestsException();
            }
            catch (HttpRequestException) when (result?.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new EndOfShowsException();
            }
        }
    }
}
