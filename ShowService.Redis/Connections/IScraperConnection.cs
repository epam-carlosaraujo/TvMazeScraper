using System.Threading.Tasks;

namespace ShowService.Redis.Connections
{
    public interface IScraperConnection
    {
        Task<int> ScrapAsync(int apiOffset, int apiCount);
    }
}
