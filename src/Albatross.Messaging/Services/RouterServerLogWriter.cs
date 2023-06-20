using Albatross.Messaging.Configurations;
using Albatross.Messaging.DataLogging;
using Microsoft.Extensions.Logging;

namespace Albatross.Messaging.Services {
	public class RouterServerLogWriter : DiskStorageLogWriter {
		public RouterServerLogWriter(RouterServerConfiguration config, ILogger<RouterServerLogWriter> logger) : base(config.DiskStorage, logger) {
		}
	}

	public class RouterServerLogReader: DiskStorageLogReader {
		public RouterServerLogReader(RouterServerConfiguration config, ILogger<RouterServerLogWriter> logger) : base(config.DiskStorage, logger) {
		}
	}
}
