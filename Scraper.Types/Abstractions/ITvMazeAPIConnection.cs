using Scraper.Types.TvMazeAPI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scraper.Types.Abstractions
{
    public interface ITvMazeAPIConnection
    {
        int ShowsPageLimit { get; }

        /// <summary>
        /// Abstraction over TvMaze API. See: <a href="http://www.tvmaze.com/api">documentation here</a>.
        /// </summary>
        /// <param name="page">The page number to get</param>
        /// <returns></returns>
        /// <exception cref="EndOfShowsException">Thrown when reaches the end of records.</exception>
        /// <exception cref="TooManyRequestsException">Throws when requests exceeds API quota.</exception>
        Task<IEnumerable<TvMazeAPITypes.Show>> GetShowsAsync(int page);
        /// <summary>
        /// Abstraction over TvMaze API. See: <a href="http://www.tvmaze.com/api">documentation here</a>.
        /// </summary>
        /// <param name="page">The page number to get</param>
        /// <returns></returns>
        /// <exception cref="EndOfShowsException">Thrown when reaches the end of records.</exception>
        /// <exception cref="TooManyRequestsException">Throws when requests exceeds API quota.</exception>
        Task<IEnumerable<TvMazeAPITypes.Cast>> GetCastByShowIdAsync(int showId);
    }
}
