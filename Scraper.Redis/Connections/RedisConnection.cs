using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Types.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Scraper.Redis.Connections
{
    public class RedisConnection : IRedisConnection
    {
        private IDatabase _database;

        public RedisConnection(IDatabase database)
        {
            _database = database;
        }

        public async Task<int> SaveToRedisAsync(List<Scraper.Types.TvMazeAPI.TvMazeAPITypes.Show> shows)
        {
            int storedCountBeforeInsert = (int)await _database.SortedSetLengthAsync(RedisKeys.Shows);
            foreach (var show in shows)
            {
                await _database.SortedSetAddAsync(RedisKeys.Shows, JsonConvert.SerializeObject(show), show.Id);
            }
            await _database.KeyExpireAsync(RedisKeys.Shows, TimeSpan.FromDays(1));
            int storedCountAfterInsert = (int)await _database.SortedSetLengthAsync(RedisKeys.Shows);
            return storedCountAfterInsert - storedCountBeforeInsert;
        }
    }
}
