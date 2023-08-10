using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Albatross.Messaging.DataLogging {
	public class DealerClientLogReader : DiskStorageLogReader {
		public DealerClientLogReader(DealerClientConfiguration config, IMessageFactory messageFactory, ILogger<DealerClientLogReader> logger) : base(config.DiskStorage, messageFactory, logger) {
		}
	}
}
