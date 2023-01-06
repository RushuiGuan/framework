using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Albatross.Caching {
	public interface IRedisKeyManagement {
		IEnumerable<string> FindKeys(string pattern);
		void FindAndRemoveKeys(string pattern);
		void Remove(params string[] keys);
	}
	public class RedisKeyManagement : IRedisKeyManagement{
		private volatile IConnectionMultiplexer? connection;
		private readonly RedisCacheOptions options;
		private List<IServer> servers = new List<IServer>();
		private List<IServer> replicas = new List<IServer>();
		private bool disposed;
		private readonly string instance;
		private readonly SemaphoreSlim connectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);
		private readonly IDistributedCache cache;

		public RedisKeyManagement(IOptions<RedisCacheOptions> options, IDistributedCache cache) {
			this.options = options.Value;
			instance = this.options.InstanceName ?? string.Empty;
			this.cache = cache;
		}

		private void Connect() {
			CheckDisposed();
			if (servers.Count > 0) {
				Debug.Assert(connection != null);
				return;
			}
			connectionLock.Wait();
			try {
				if (servers.Count == 0) {
					if (options.ConnectionMultiplexerFactory == null) {
						if (options.ConfigurationOptions is not null) {
							connection = ConnectionMultiplexer.Connect(options.ConfigurationOptions);
						} else {
							connection = ConnectionMultiplexer.Connect(options.Configuration);
						}
					} else {
						connection = options.ConnectionMultiplexerFactory().GetAwaiter().GetResult();
					}
					var endpoints = connection.GetEndPoints();
					foreach (var item in endpoints) {
						var server = connection.GetServer(item);
						if (server.IsReplica) {
							this.replicas.Add(server);
						} else { 
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
		public IEnumerable<string> FindKeys(string pattern) {
			if(string.IsNullOrEmpty(pattern)) {
				throw new ArgumentException("Key pattern cannot be null or empty string");
			}
			Connect();
			pattern = instance + pattern;
			List<string> keys = new List<string>();
			foreach (var server in servers) {
				foreach (var key in server.Keys(pattern: pattern)) {
					keys.Add(key.ToString().Substring(instance.Length));
				}
			}
			return keys;
		}

		public void FindAndRemoveKeys(string pattern) {
			var keys = this.FindKeys(pattern);
			foreach(var item in keys) {
				this.cache.Remove(item);
			}
		}
		public void Remove(params string[] keys) {
			foreach (var key in keys) {
				this.cache.Remove(key);
			}
		}
		private void CheckDisposed() {
			if (disposed) {
				throw new ObjectDisposedException(this.GetType().FullName);
			}
		}
		public void Dispose() {
			if (disposed) { return; }
			disposed = true;
			connection?.Close();
		}
	}
}