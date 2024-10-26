using System;

namespace Albatross.Messaging.Services {
	public enum ClientState {
		Dead = 0,
		Alive = 1,
	}

	public class Client {
		public string Identity { get; init; }
		public ClientState State { get; private set; }
		public DateTime LastHeartbeat { get; private set; }

		public Client(string identity) {
			this.Identity = identity;
			this.State = ClientState.Alive;
			this.LastHeartbeat = DateTime.UtcNow;
		}

		public void Connected() {
			this.State = ClientState.Alive;
			this.LastHeartbeat = DateTime.UtcNow;
		}
		public void Lost() {
			this.State = ClientState.Dead;
		}

		public void UpdateHeartbeat() {
			LastHeartbeat = DateTime.UtcNow;
		}
	}
}