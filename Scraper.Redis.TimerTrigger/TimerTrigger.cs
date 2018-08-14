using System;
using System.Threading.Tasks;
using AzureFunctions.Autofac;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Scraper.Redis.AzureFunctions.DI;
using Scraper.Types;
using Scraper.Types.Abstractions;

namespace Scraper.Redis.TimerTrigger
{
    [DependencyInjectionConfig(typeof(InjectConfiguration))]
    public static class TimerTrigger
    {
        [FunctionName("TimerTrigger")]
        public static async Task Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger log, [Inject]IScraper scraper)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            int scraped = 0;
            ScraperResult result = null;
            do
            {
                result = await scraper.ScrapAsync(0, -1);
                scraped += result.NumberOfRecordsScraped;
            }
            while (!result.EndOfScraping);
            log.LogInformation($"Scraping has finished at: {DateTime.Now}. {scraped} records were scraped.");
        }
    }
}
