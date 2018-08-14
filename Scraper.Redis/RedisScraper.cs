using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scraper.Redis.Connections;
using Scraper.Types;
using Scraper.Types.Abstractions;
using Scraper.Types.Exceptions;

namespace Scraper.Redis
{
    public class RedisScraper : IScraper
    {
        private ITvMazeAPIConnection _api;
        private IRedisConnection _redis;
        private int _pageLimit;

        public RedisScraper(IRedisConnection redis, ITvMazeAPIConnection api)
        {
            if (api.ShowsPageLimit <= 0)
            {
                throw new ArgumentException("Page limit for API shows endpoint must be a positive number.", nameof(api));
            }
            _pageLimit = api.ShowsPageLimit;
            _api = api;
            _redis = redis;
        }

        public async Task<ScraperResult> ScrapAsync(int offset, int count)
        {
            offset = offset >= 0 ? offset : throw new ArgumentOutOfRangeException(nameof(offset));
            var shows = new List<Scraper.Types.TvMazeAPI.TvMazeAPITypes.Show>();
            int pages = count < 0 ? (int.MaxValue / _pageLimit) : ((offset + count) / _pageLimit) + 1;
            int remainingToTake = count;
            var result = new ScraperResult();
            try
            {
                for (var page = 0; page < pages; page++)
                {
                    var showsFromThisPage = (await _api.GetShowsAsync(page)).ToArray();
                    var showsFromThisPageCount = showsFromThisPage.Count();
                    for (var i = 0; i < showsFromThisPageCount; i++)
                    {
                        if (((i >= offset) && ((count < 0) || (remainingToTake > 0))))
                        {
                            showsFromThisPage[i].Cast = await _api.GetCastByShowIdAsync(showsFromThisPage[i].Id);
                            shows.Add(showsFromThisPage[i]);
                            remainingToTake--;
                        }
                    }
                    offset = Math.Max(offset - showsFromThisPageCount, 0);
                }
            }
            catch (EndOfShowsException)
            {
                result.EndOfScraping = true;
            }
            catch (TooManyRequestsException)
            {
                result.EndOfScraping = false;
            }
            result.NumberOfRecordsScraped = await _redis.SaveToRedisAsync(shows);
            return result;
        }
    }
}
