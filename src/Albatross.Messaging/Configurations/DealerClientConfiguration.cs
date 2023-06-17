using Albatross.Config;

namespace Albatross.Messaging.Configurations {
	public record class DealerClientConfiguration {
		public string Identity { get; set; } = string.Empty;
		public string EndPoint { get; set; } = string.Empty;
		public int AckTimeout { get;set; }
		public bool UseHeartbeat { get; set; }

		/// <summary>
		/// Timer interval in milliseconds.  If not specifed, default to constant <see cref="RouterServerConfiguration.DefaultTimerInterval"/>.
		/// </summary>
		public int? TimerInterval { get; set; }
		public const int DefaultTimerInterval = 5000;

		public DiskStorageConfiguration DiskStorage { get; set; } = new DiskStorageConfiguration(null, "dealer-client");
	}
}
