using Microsoft.Extensions.Logging.Console;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Caching.Utility {
	public class RedisConnection : IRedisConnection {
		private bool _disposed;
		
		public IConnectionMultiplexer Connection { get; init; }
		public IDatabase Database { get; init; }

		public RedisConnection(RedisConfig config) {
			var option = new ConfigurationOptions {
				EndPoints = {config.RedisConnectionString},
				User = config.User,
				Password = config.Password,
				AllowAdmin = false,
			};
			Connection = ConnectionMultiplexer.Connect(option);
			this.Database = Connection.GetDatabase();
		}

		public void Dispose() {
			if(!_disposed) { 
				_disposed = true;
				Connection?.Dispose();
			}
		}
	}
}
