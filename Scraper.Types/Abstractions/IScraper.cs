using System.Threading.Tasks;

namespace Scraper.Types.Abstractions
{
    public interface IScraper
    {
        /// <summary>
        /// Scraps API for Shows.
        /// </summary>
        /// <param name="offset">Number of records to skip before start to take.</param>
        /// <param name="count">Number of records to take. Pass -1 to get until the end of the stream.</param>
        /// <returns>The number of records scraped</returns>
        Task<ScraperResult> ScrapAsync(int offset, int count);
    }
}