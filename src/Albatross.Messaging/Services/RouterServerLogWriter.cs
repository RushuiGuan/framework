using Albatross.Messaging.Configurations;
using Albatross.Messaging.DataLogging;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Albatross.Messaging.Services {
	public class RouterServerLogWriter : DiskStorageLogWriter {
		public RouterServerLogWriter(RouterServerConfiguration config, ILogger<RouterServerLogWriter> logger, BufferedTextWriter lineWriter) : base(config.DiskStorage, logger, lineWriter) {
		}
	}

	public class RouterServerLogReader: DiskStorageLogReader {
		public RouterServerLogReader(RouterServerConfiguration config, IMessageFactory messageFactory, ILogger<RouterServerLogWriter> logger) : base(config.DiskStorage, messageFactory, logger) {
		}
	}
}
