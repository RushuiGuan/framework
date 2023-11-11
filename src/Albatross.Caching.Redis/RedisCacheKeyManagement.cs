using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Caching.Redis {
	public class RedisCacheKeyManagement : ICacheKeyManagement {
		private volatile IConnectionMultiplexer? connection;
		private readonly RedisCacheOptions options;
		private List<IServer> servers = new List<IServer>();
		private List<IServer> replicas = new List<IServer>();
		private bool disposed;
		private readonly string instance;
		private readonly SemaphoreSlim connectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);
		private readonly IDistributedCache cache;
		private readonly ILogger<RedisCacheKeyManagement> logger;

		public RedisCacheKeyManagement(IOptions<RedisCacheOptions> options, IDistributedCache cache, ILogger<RedisCacheKeyManagement> logger) {
			this.options = options.Value;
			instance = this.options.InstanceName ?? string.Empty;
			this.cache = cache;
			this.logger = logger;
			logger.LogInformation("RedisCacheInstance has been created with instance={instance}", instance.TrimEnd(':'));
		}

		private async ValueTask ConnectAsync() {
			if (disposed) { throw new ObjectDisposedException(GetType().FullName); }
			if (servers.Count > 0) {
				Debug.Assert(connection != null);
				return;
			}
			logger.LogInformation("Connecting to redis server");
			await connectionLock.WaitAsync();
			try {
				if (servers.Count == 0) {
					if (options.ConnectionMultiplexerFactory == null) {
						if (options.ConfigurationOptions is not null) {
							connection = await ConnectionMultiplexer.ConnectAsync(options.ConfigurationOptions);
						} else {
							connection = await ConnectionMultiplexer.ConnectAsync(options.Configuration);
						}
					} else {
						connection = await options.ConnectionMultiplexerFactory();
					}
					var endpoints = connection.GetEndPoints();
					foreach (var item in endpoints) {
						var server = connection.GetServer(item);
						if (server.IsReplica) {
							logger.LogInformation("Connected to replica {name}", server);
							replicas.Add(server);
						} else {
							logger.LogInformation("Connected to server {name}", server);
							servers.Add(server);
						}
					}
				}
			} finally {
				connectionLock.Release();
			}
			Debug.Assert(connection != null);
		}
		/// <summary>
		/// return all keys in redis that matched the provided pattern for this application.  Note that instanceName is automatically prefixed when searching redis.
		/// The method will also return the keys without the instanceName prefix
		/// </summary>
		/// <param name="pattern"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public async ValueTask<IEnumerable<string>> FindKeys(string pattern) {
			if (string.IsNullOrEmpty(pattern)) {
				throw new ArgumentException("Key pattern cannot be null or empty string");
			}
			await ConnectAsync();
			pattern = instance + pattern;
			logger.LogInformation("Searching keys with pattern: {value}", pattern);
			List<string> keys = new List<string>();
			foreach (var server in servers) {
				foreach (var key in server.Keys(pattern: pattern)) {
					keys.Add(key.ToString().Substring(instance.Length));
				}
			}
			return keys;
		}

		public async ValueTask FindAndRemoveKeys(string pattern) {
			var keys = await FindKeys(pattern);
			logger.LogInformation("Removing redis cache keys: {@key}", keys);
			foreach (var item in keys) {
				cache.Remove(item);
			}
		}
		public void Remove(IEnumerable<string> keys) {
			logger.LogInformation("Removing redis cache keys: {@key}", keys);
			foreach (var key in keys) {
				cache.Remove(key);
			}
		}
		public void Dispose() {
			if (disposed) { return; }
			logger.LogInformation("Closing redis connection");
			disposed = true;
			connection?.Close();
		}
	}
}