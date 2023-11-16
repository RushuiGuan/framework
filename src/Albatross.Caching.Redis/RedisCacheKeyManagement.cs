using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Caching.Redis {
	public class RedisCacheKeyManagement : ICacheKeyManagement, IDisposable {
		private bool disposed;
		private IConnectionMultiplexer? connection;
		private IDatabase? cache;
		private List<IServer> servers = new List<IServer>();
		private List<IServer> replicas = new List<IServer>();

		private readonly RedisCacheOptions options;
		private readonly string instance;
		private readonly ILogger<RedisCacheKeyManagement> logger;

		public IConnectionMultiplexer Connection => this.connection ?? throw new InvalidOperationException("Redis connection not initialized");
		public IDatabase Cache => this.cache ?? throw new InvalidOperationException("Redis connection not initialized");

		public RedisCacheKeyManagement(IOptions<RedisCacheOptions> options, ILogger<RedisCacheKeyManagement> logger) {
			this.options = options.Value;
			instance = this.options.InstanceName ?? string.Empty;
			this.logger = logger;
			logger.LogInformation("RedisCacheInstance has been created with instance={instance}", instance.TrimEnd(':'));
		}

		public async Task Init() {
			if (disposed) { throw new ObjectDisposedException(GetType().FullName); }
			if (servers.Count > 0) {
				Debug.Assert(connection != null);
				Debug.Assert(cache != null);
				return;
			}
			logger.LogInformation("Connecting to redis server");
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
				this.cache = connection.GetDatabase();
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
			Debug.Assert(connection != null);
		}

		/// <summary>
		/// return all keys in redis that matched the provided pattern for this application.  Note that instanceName is automatically prefixed when searching redis.
		/// The method will also return the keys without the instanceName prefix
		/// </summary>
		/// <param name="pattern"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public string[] FindKeys(string pattern) {
			if (string.IsNullOrEmpty(pattern)) {
				throw new ArgumentException("Key pattern cannot be null or empty string");
			}else if(servers.Count == 0) {
				throw new InvalidOperationException("Redis connection not initialized");
			}
			pattern = instance + pattern;
			logger.LogInformation("Searching keys with pattern: {value}", pattern);
			List<string> keys = new List<string>();
			foreach (var server in servers) {
				foreach (var key in server.Keys(pattern: pattern)) {
					keys.Add(key.ToString().Substring(instance.Length));
				}
			}
			return keys.ToArray();
		}

		public void Remove(string[] keys) {
			if (keys.Length > 0) {
				logger.LogInformation("Removing redis cache keys: {@keys}", keys);
				this.Cache.KeyDelete(keys.Select(key => new RedisKey(instance + key)).ToArray());
			}
		}
		public void Dispose() {
			if (disposed) { return; }
			logger.LogInformation("Closing redis connection");
			connection?.Close();
			disposed = true;
		}
	}
}