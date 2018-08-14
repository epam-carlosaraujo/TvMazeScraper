using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Types.Abstractions;
using Common.Types.Data;
using ShowService.Redis.Connections;

namespace ShowService.Redis
{
    public class RedisShowService : IShowService
    {
        private IRedisConnection _redis;
        private IScraperConnection _scraper;
        private List<Show> _shows;

        public RedisShowService(IScraperConnection scraper, IRedisConnection redis)
        {
            _redis = redis;
            _scraper = scraper;
            _shows = new List<Show>();
        }

        public async Task<IEnumerable<Show>> GetListAsync(int offset, int count)
        {
            _shows.AddRange(await _redis.GetShowsFromRedisAsync(offset, count));
            if (_shows.Count < Math.Max(count, 0))
            {
                int redisHasCount = await _redis.ShowsInRedisAsync();
                int apiOffset = offset + _shows.Count;
                int apiCount = count - _shows.Count;
                int scraped = await _scraper.ScrapAsync(apiOffset, apiCount);
                _shows.AddRange(await _redis.GetShowsFromRedisAsync(redisHasCount, scraped));
            }
            return _shows;
        }
    }
}
