using Albatross.Config;

namespace Albatross.Messaging.Configurations {
	public class RouterServerConfiguration {
		public string EndPoint { get; set; } = string.Empty;
		public int SendHighWatermark { get; set; }
		public int ReceiveHighWatermark { get; set; }
		/// <summary>
		/// when starting up, the number of minutes in logs to replay
		/// </summary>
		public int LogCatchUpPeriod { get; set; }

		/// <summary>
		/// Timer interval in milliseconds.  If not specifed, default to constant <see cref="RouterServerConfiguration.DefaultTimerInterval"/>.
		/// </summary>
		public int? TimerInterval { get; set; }
		public const int DefaultTimerInterval = 5000;

		public DiskStorageConfiguration DiskStorage { get; set; } = new DiskStorageConfiguration(null, "router-server");
	}
}
