using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Types.Data;

namespace ShowService.Redis.Connections
{
    public interface IRedisConnection
    {
        Task<IEnumerable<Show>> GetShowsFromRedisAsync(int offset, int count);
        Task<int> ShowsInRedisAsync();
    }
}
