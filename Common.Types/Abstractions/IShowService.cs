using Common.Types.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Types.Abstractions
{
    public interface IShowService
    {
        /// <summary>
        /// Get a list of Show.
        /// </summary>
        /// <param name="offset">Number of records to skip before start to take.</param>
        /// <param name="count">Number of records to take. Pass -1 to get until the end of the stream.</param>
        /// <returns></returns>
        /// <exception cref="PartialResultException{T}">Throw when backend API is busy with too many requests</exception>
        Task<IEnumerable<Show>> GetListAsync(int offset, int count);
    }
}