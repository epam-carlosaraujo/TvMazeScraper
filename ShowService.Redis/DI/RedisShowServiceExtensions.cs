using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using ShowService.Redis.Connections;
using Common.Types.Network;
using Common.Types.Abstractions;

namespace ShowService.Redis.DI
{
    public static class RedisShowServiceExtensions
    {
        public static IServiceCollection AddRedisShowService(this IServiceCollection services, NetworkConfiguration networkConfiguration, HttpConfiguration httpConfiguration)
        {
            services.AddSingleton(httpConfiguration);
            services.AddSingleton(networkConfiguration);
            RegisterHttp(services);
            RegisterRedis(services, networkConfiguration.RedisConnection);
            RegisterRedisService(services);
            return services;
        }

        private static void RegisterHttp(IServiceCollection services)
        {
            services.AddTransient<IHttpHandler, HttpClientHandler>();
        }

        private static void RegisterRedis(IServiceCollection services, string redisConnection)
        {
            services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(redisConnection));
            services.AddTransient(provider => provider.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
        }

        private static void RegisterRedisService(IServiceCollection services)
        {
            services.AddTransient<IScraperConnection, ScraperConnection>();
            services.AddTransient<IRedisConnection, RedisConnection>();
            services.AddTransient<IShowService, RedisShowService>();
        }
    }
}
