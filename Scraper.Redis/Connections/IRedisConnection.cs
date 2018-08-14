using Scraper.Types.TvMazeAPI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scraper.Redis.Connections
{
    public interface IRedisConnection
    {
        /// <summary>
        /// Insert shows into Redis.
        /// </summary>
        /// <param name="shows"></param>
        /// <returns>The number of records inserted.</returns>
        Task<int> SaveToRedisAsync(List<TvMazeAPITypes.Show> shows);    
    }
}
