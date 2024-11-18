using Albatross.Config;
using System;

namespace Albatross.Messaging.Configurations {
	public class RouterServerConfiguration {
		public string EndPoint { get; set; } = string.Empty;
		public int SendHighWatermark { get; set; }
		public int ReceiveHighWatermark { get; set; }
		/// <summary>
		/// when starting up, the number of minutes in logs to replay
		/// </summary>
		public int LogCatchUpPeriod { get; set; }
		public bool UseCurveEncryption { get; set; }
		public string? ServerPrivateKey { get; set; }

		/// <summary>
		/// Timer interval in milliseconds.  If not specifed, default to 3 seconds
		/// </summary>
		public int? TimerInterval { get; set; }
		public const int DefaultTimerInterval = 3000;
		public int ActualTimerInterval => this.TimerInterval ?? DefaultTimerInterval;

		public DiskStorageConfiguration DiskStorage { get; set; } = new DiskStorageConfiguration(null, "router-server");
		/// <summary>
		/// when set to true, connectivity with server is maintained using heartbeat.  If client and server resides within the same application, this flag can be set to false
		/// </summary>
		public bool MaintainConnection { get; set; }

		public int? HeartbeatThreshold { get; set; }
		Lazy<TimeSpan> heartbeatThresholdTimeSpan;
		public TimeSpan HeartbeatThresholdTimeSpan => this.heartbeatThresholdTimeSpan.Value;

		public RouterServerConfiguration() {
			this.heartbeatThresholdTimeSpan = new Lazy<TimeSpan>(() => TimeSpan.FromMilliseconds(this.HeartbeatThreshold ?? this.ActualTimerInterval * 1.2), false);
		}
	}
}