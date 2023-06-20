using Albatross.Messaging.Configurations;
using Albatross.Messaging.DataLogging;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Albatross.Messaging.Services {
	public class DealerClientLogWriter : DiskStorageLogWriter {
		public DealerClientLogWriter(DealerClientConfiguration config, ILogger<DealerClientLogWriter> logger) : base(config.DiskStorage, logger) {
		}
	}

	public class DealerClientLogReader : DiskStorageLogReader {
		public DealerClientLogReader(DealerClientConfiguration config, ILogger<DealerClientLogWriter> logger) : base(config.DiskStorage, logger) {
		}
	}
}
