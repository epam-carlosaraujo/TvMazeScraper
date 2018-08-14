using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Common.Types.Data;
using Common.Types.Configuration;

namespace ShowService.Redis.Connections
{
    public class RedisConnection : IRedisConnection
    {
        private IDatabase _database;

        public RedisConnection(IDatabase database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Show>> GetShowsFromRedisAsync(int offset, int count)
        {
            if (count == 0)
            {
                return Enumerable.Empty<Show>();
            }
            int stop = count < 0 ? -1 : (offset + count) - 1;
            var data = await _database.SortedSetRangeByRankAsync(RedisKeys.Shows, offset, stop);
            if (data.Length > 0)
            {
                return data.Select(show => JsonConvert.DeserializeObject<Show>(show));
            }
            else
            {
                return Enumerable.Empty<Show>();
            }
        }

        public async Task<int> ShowsInRedisAsync()
        {
            return (int)await _database.SortedSetLengthAsync(RedisKeys.Shows);
        }
    }
}
