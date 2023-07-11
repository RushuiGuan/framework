using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Albatross.Messaging.DataLogging {
	public class RouterServerLogWriter : DiskStorageLogWriter {
		public RouterServerLogWriter(RouterServerConfiguration config, ILoggerFactory loggerFactory) : base("routerserver", config.DiskStorage, loggerFactory) {
		}
	}

	public class RouterServerLogReader : DiskStorageLogReader {
		public RouterServerLogReader(RouterServerConfiguration config, IMessageFactory messageFactory, ILogger<RouterServerLogReader> logger) : base(config.DiskStorage, messageFactory, logger) {
		}
	}
}
