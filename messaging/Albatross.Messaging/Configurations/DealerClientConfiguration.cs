using System;

namespace Albatross.Messaging.Configurations {
	public record class DealerClientConfiguration {
		public string LogFileName { get => this.DiskStorage.FileName; set => this.DiskStorage.FileName = value; }
		public string Identity { get; set; } = string.Empty;
		public string EndPoint { get; set; } = string.Empty;
		public int AckTimeout { get; set; }
		public bool UseCurveEncryption { get; set; }
		public string? ServerPublicKey { get; set; }

		/// <summary>
		/// Timer interval in milliseconds.  If not specifed, default to 3 seconds
		/// </summary>
		public int? TimerInterval { get; set; }
		const int DefaultTimerInterval = 3000;
		public int ActualTimerInterval => TimerInterval ?? DefaultTimerInterval;

		public DiskStorageConfiguration DiskStorage { get; set; } = new DiskStorageConfiguration(null, "dealer-client");

		/// <summary>
		/// when set to true, connectivity with server is maintained using heartbeat.  If client and server resides within the same application, this flag can be set to false
		/// </summary>
		public bool MaintainConnection { get; set; }
		/// <summary>
		/// this value is used a lot in runtime and should be cached
		/// The max duration of time in milliseconds can ellapse without an heartbeat to cause a disconnect
		/// This number has to be larger than the heartbeat interval.  If not set, it is 1.2 times of HeartbeatInterval
		/// Relax the threshold if there is large distance between two services.
		/// </summary>
		public int? HeartbeatThreshold { get; set; }
		Lazy<TimeSpan> heartbeatThresholdTimeSpan;
		public TimeSpan HeartbeatThresholdTimeSpan => this.heartbeatThresholdTimeSpan.Value;

		public DealerClientConfiguration() {
			this.heartbeatThresholdTimeSpan = new Lazy<TimeSpan>(() => TimeSpan.FromMilliseconds(this.HeartbeatThreshold ?? this.ActualTimerInterval * 1.2), false);
		}
	}
}