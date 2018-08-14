using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Scraper.Types.Abstractions;
using AzureFunctions.Autofac;
using Scraper.Redis.AzureFunctions.DI;

namespace Scraper.Redis.HttpTrigger
{
    [DependencyInjectionConfig(typeof(InjectConfiguration))]
    public static class HttpTrigger
    {
        [FunctionName("HttpTrigger")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "HttpScraper")]HttpRequest req, ILogger log, [Inject]IScraper scraper)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            TryGetParamsFromQuery(req, out int offset, out int count);
            var result = await scraper.ScrapAsync(offset, count);
            return new OkObjectResult(result.NumberOfRecordsScraped);
        }

        private static void TryGetParamsFromQuery(HttpRequest req, out int offset, out int count)
        {
            req.Query.TryGetValue("offset", out StringValues offsetParam);
            int.TryParse(offsetParam, out offset);
            req.Query.TryGetValue("count", out StringValues countParam);
            int.TryParse(countParam, out count);
        }
    }
}
