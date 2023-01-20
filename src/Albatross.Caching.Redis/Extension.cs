using Albatross.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly.Caching;
using System;

namespace Albatross.Caching.Redis {
	public static class Extension {
		public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration) {
			var config = new RedisConfig(configuration);
			config.Validate();

			services.AddConfig<RedisConfig>();
			services.AddSingleton<ICacheKeyManagement, RedisCacheKeyManagement>();
			services.TryAdd(ServiceDescriptor.Singleton<IAsyncCacheProvider<string>, Polly.Caching.Distributed.NetStandardIDistributedCacheStringProvider>());
			services.TryAdd(ServiceDescriptor.Singleton<IAsyncCacheProvider<byte[]>, Polly.Caching.Distributed.NetStandardIDistributedCacheByteArrayProvider>());
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
	}
}
