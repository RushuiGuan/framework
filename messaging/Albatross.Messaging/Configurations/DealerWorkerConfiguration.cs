using System.Collections.Generic;

namespace Albatross.Messaging.Configurations {
	public class DealerWorkerConfiguration {
		public const string DefaultService = "default";
		public string Identity { get; init; }
		public string EndPoint { get; set; }
		public ISet<string> Services { get; init; } = new HashSet<string> { DefaultService };
		public DealerWorkerConfiguration(string identity, string endpoint) {
			this.Identity = identity;
			this.EndPoint = endpoint;
		}
		public int? HeartbeatInterval { get; set; }
		/// <summary>
		/// The max duration of time in milliseconds can ellapse without an heartbeat to cause a disconnect
		/// This number has to be larger than HeartbeatInterval.  If not set, it is 1.2 times of HeartbeatInterval
		/// Relax the threshold if there is large distance between two services.
		/// </summary>
		public int? HeartbeatThreshold { get; set; }
		public double ActualHeartbeatInterval => HeartbeatInterval ?? 3000;
		public double ActualHeartbeatThreshold => HeartbeatThreshold ?? ActualHeartbeatInterval * 1.2;
	}
}