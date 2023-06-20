using System;

namespace Albatross.Messaging.Services {
	public enum ClientState {
		Unknown = 0,
		Connected = 1,
	}

	public class Client {
		public string Identity { get; init; }
		public ClientState State { get; private set; }
		public DateTime LastHeartbeat { get; private set; }

		public Client(string identity) {
			this.Identity = identity;
		}

		public void Connected() {
			this.State = ClientState.Connected;
			this.LastHeartbeat = DateTime.UtcNow;
		}
		public void Lost() {
			this.State = ClientState.Unknown;
		}

		public void Heartbeat() {
			LastHeartbeat = DateTime.Now;
		}
	}
}
