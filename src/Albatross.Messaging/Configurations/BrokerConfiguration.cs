namespace Albatross.Messaging.Configurations {
	public class BrokerConfiguration {
		public string Identity { get; set; } = string.Empty;
		public string EndPoint { get; set; } = string.Empty;
		public bool Durable { get; set; }
		/// <summary>
		/// heart beat interval in milliseconds, if not set, default to 3000 ms
		/// </summary>
		public int? HeartbeatInterval { get; set; }
		/// <summary>
		/// The max duration of time in milliseconds can ellapse without an heartbeat to cause a disconnect
		/// This number has to be larger than HeartbeatInterval.  If not set, it is 1.2 times of HeartbeatInterval
		/// Relax the threshold if there is large distance between two services.
		/// </summary>
		public int? HeartbeatThreshold { get; set; }
		public double ActualHeartbeatInterval => HeartbeatInterval ?? 3000;
		public double ActualHeartbeatThreshold => HeartbeatThreshold ?? ActualHeartbeatInterval * 1.2;
		public bool AllowParallelExecution { get; set; }
		public bool UseHeartbeat { get; set; }
	}
}