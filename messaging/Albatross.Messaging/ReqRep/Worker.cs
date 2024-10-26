using Albatross.Messaging.ReqRep.Messages;
using System;
using System.Collections.Generic;

namespace Albatross.Messaging.ReqRep {
	public enum WorkerState {
		Unavailable = 0,
		Connected = 1,
	}
	public class Worker {
		public string Identity { get; init; }
		public WorkerState State { get; set; }
		public DateTime LastHeartbeat { get; set; }
		public List<ClientRequest> Requests { get; init; } = new List<ClientRequest>();

		public Worker(string identity) {
			Identity = identity;
		}
	}
}