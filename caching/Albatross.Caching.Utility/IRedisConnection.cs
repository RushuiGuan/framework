using StackExchange.Redis;
using System;

namespace Albatross.Caching.Utility {
	public interface IRedisConnection : IDisposable{
		public IConnectionMultiplexer Connection { get; }
		public IDatabase Database { get; }
	}
}