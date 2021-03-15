using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public interface IMemoryCacheExtended {
		IEnumerable<object> Keys { get; }
		void Envict(IEnumerable<object> keys);
		void Reset();
	}
	/// <summary>
	/// an extension to the MemoryCache class that provides the ability to retrieve cache keys and reset cache
	/// </summary>
	public class MemoryCacheExtended : IMemoryCacheExtended {
		private readonly MemoryCache cache;
		private readonly IHttpClientFactory clientFactory;
		private readonly CachingConfig config;
		private readonly ILogger<MemoryCacheExtended> logger;

		public MemoryCacheExtended(IMemoryCache cache, IHttpClientFactory clientFactory, CachingConfig config, ILogger<MemoryCacheExtended> logger) {
			if(!(cache is MemoryCache)) {
				throw new InvalidOperationException("MemoryCacheReset can only be used with MemoryCache");
			}

			this.cache = (MemoryCache)cache;
			this.clientFactory = clientFactory;
			this.config = config;
			this.logger = logger;
		}

		public IEnumerable<object> Keys {
			get {
				FieldInfo field = typeof(MemoryCache).GetField("_entries", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				IDictionary dictionary = (IDictionary)field.GetValue(cache);
				return dictionary.Keys.Cast<object>();
			}
		}

		public void Envict(IEnumerable<object> keys) {
			foreach (string key in keys) { this.cache.Remove(key); }
			_ = EnvictRemoteCache(keys);
		}

		private async Task EnvictRemoteCache(IEnumerable<object> keys) {
			foreach (string instance in config.OtherInstances) {
				try { 
					using var client = clientFactory.CreateClient(CachingClient.CachingProxyName);
					var proxy = new CachingClient(logger, client);
					await proxy.Envict(keys);
				} catch (Exception err) {
					logger.LogError(err, "Error envicting caching for instance {name}", instance);
				}
			}
		}

		public void Reset() {
			foreach (object key in Keys) { this.cache.Remove(key); }
			_ = ResetRemoteCache();
		}

		private async Task ResetRemoteCache() {
			foreach (string instance in config.OtherInstances) {
				try {
					using var client = clientFactory.CreateClient(CachingClient.CachingProxyName);
					var proxy = new CachingClient(logger, client);
					await proxy.Reset();
				}catch(Exception err) {
					logger.LogError(err, "Error resetting caching for instance {name}", instance);
				}
			}
		}
	}
}
