using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Types.Abstractions
{
    public interface IHttpHandler
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
