using Albatross.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace Albatross.Caching.Redis {
	public static class Extensions {
		public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration) {
			var config = new RedisConfig(configuration);
			config.Validate();

			services.AddConfig<RedisConfig>();
			services.TryAddSingleton<RedisCacheKeyManagement>();
			services.TryAddSingleton<Polly.Caching.Distributed.NetStandardIDistributedCacheStringProvider>();
			services.TryAddSingleton<Polly.Caching.Distributed.NetStandardIDistributedCacheByteArrayProvider>();
			services.AddSingleton<ICacheKeyManagement>(provider => provider.GetRequiredService<RedisCacheKeyManagement>());
			services.AddStackExchangeRedisCache(option => {
				option.InstanceName = config.InstanceName;
				option.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions {
					EndPoints = { config.RedisConnectionString },
					User = config.User,
					Password = config.Password,
					// need this for the KEYs command
					AllowAdmin = true,
					Ssl = false,
				};
			});
			switch (config.RedisValueFormat) {
				case RedisValueFormat.String:
					services.AddSingleton<ICacheProviderAdapter, StringCacheProviderAdapter>();
					break;
				case RedisValueFormat.Bytes:
					services.AddSingleton<ICacheProviderAdapter, ByteArrayCacheProviderAdapter>();
					break;
				default:
					throw new NotSupportedException();
			}
			return services;
		}
		public static void UseRedisCaching(this IServiceProvider serviceProvider) {
			var logger = serviceProvider.GetRequiredService<ILogger>();
			try {
				serviceProvider.GetRequiredService<RedisCacheKeyManagement>().Init().Wait();
			}catch(Exception err) {
				logger.LogError(err, "Error connecting to redis server");
			}
		}
	}
}
