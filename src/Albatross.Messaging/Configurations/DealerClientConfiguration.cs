using Albatross.Config;

namespace Albatross.Messaging.Configurations {
	public record class DealerClientConfiguration {
		public string Identity { get; set; } = string.Empty;
		public string EndPoint { get; set; } = string.Empty;
		public int AckTimeout { get;set; }
		public bool UseHeartbeat { get; set; }

		public DiskStorageConfiguration DiskStorage { get; set; } = new DiskStorageConfiguration(null, "dealer-client");
	}
}
