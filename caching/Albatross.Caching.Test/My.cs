using Albatross.Caching.Redis;
using Albatross.Caching.MemCache;
using Albatross.Testing.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Albatross.Caching.Test {
	public static class My {
		public const string RedisCacheHostType = "redis";
		public const string MemCacheHostType = "memcache";

		public static void RegisterRedisServices(IConfiguration configuration, IServiceCollection services) {
			services.AddBuiltInCache();
			services.AddCaching();
			services.AddRedisCaching(configuration);
			services.AddMemCachingAsSecondary();
		}

		public static void RegisterMemCachingServices(IConfiguration configuration, IServiceCollection services) {
			services.AddCaching();
			services.AddBuiltInCache();
			services.AddMemCaching();
		}
		public static IHost GetTestHost(string type) {
			switch (type) {
				case MemCacheHostType:
					return new TestHostBuilder().WithLogging().RegisterServices(RegisterMemCachingServices).Build();
				case RedisCacheHostType:
					var host = new TestHostBuilder().WithLogging()
						.WithAppSettingsConfiguration("testing")
						.RegisterServices(RegisterRedisServices).Build();
					host.Services.UseRedisCaching();
					return host;
				default:
					throw new NotSupportedException();
			}
		}
	}
}
