using Autofac;
using AzureFunctions.Autofac.Configuration;
using Common.Types.Abstractions;
using Scraper.Redis.Connections;
using Scraper.Types.Abstractions;
using Scraper.Types.TvMazeAPI;
using StackExchange.Redis;
using System;
using Common.Types.Network;

namespace Scraper.Redis.AzureFunctions.DI
{
    public class InjectConfiguration
    {
        public InjectConfiguration(string functionName)
        {
            DependencyInjection.Initialize(builder =>
            {
                RegisterHttpConfiguration(builder);
                GetTvMazeAPIConfiguration(builder);
                RegisterHttpHandler(builder);
                RegisterRedis(builder);
                RegisterRedisConnection(builder);
                RegisterTVMazeAPI(builder);
                RegisterScraper(builder);
            }, functionName);
        }

        private static void RegisterRedisConnection(ContainerBuilder builder)
        {
            builder.Register<IRedisConnection>(provider => new RedisConnection(provider.Resolve<IDatabase>()));
        }

        private static void GetTvMazeAPIConfiguration(ContainerBuilder builder)
        {
            string cast = Environment.GetEnvironmentVariable("TvMazeAPIEndpoints_Cast");
            string shows = Environment.GetEnvironmentVariable("TvMazeAPIEndpoints_Shows");
            int pageLimit = int.Parse(Environment.GetEnvironmentVariable("TvMazeAPIEndpoints_PageLimit"));
            var apiConfig = new TvMazeAPIConfiguration() { Endpoints = new TvMazeAPIEndpoints { Cast = cast, Shows = shows }, ShowsPageLimit = pageLimit };
            builder.RegisterInstance(apiConfig);
        }

        private static void RegisterTVMazeAPI(ContainerBuilder builder)
        {
            builder.RegisterType<TvMazeAPIConnection>().As<ITvMazeAPIConnection>();
        }

        private static void RegisterScraper(ContainerBuilder builder)
        {
            builder.RegisterType<RedisScraper>().As<IScraper>();
        }

        private static void RegisterRedis(ContainerBuilder builder)
        {
            var redisConnectionString = Environment.GetEnvironmentVariable("redis");
            builder.Register<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(redisConnectionString));
            builder.Register(provider => provider.Resolve<IConnectionMultiplexer>().GetDatabase());
        }

        private static void RegisterHttpHandler(ContainerBuilder builder)
        {
            builder.RegisterType<HttpClientHandler>().As<IHttpHandler>();
        }

        private static void RegisterHttpConfiguration(ContainerBuilder builder)
        {
            int circuitBreakerLimit = int.Parse(Environment.GetEnvironmentVariable("Http_CircuitBreakerLimit"));
            int httpRetryTimes = int.Parse(Environment.GetEnvironmentVariable("Http_RetryTimes"));
            HttpConfiguration config = new HttpConfiguration { CircuitBreakerLimit = circuitBreakerLimit, HttpRetryTimes = httpRetryTimes };
            builder.RegisterInstance(config);
        }
    }
}
