using Albatross.Messaging.ReqRep.Messages;
using System;
using System.Collections.Generic;

namespace Albatross.Messaging.ReqRep {
	public enum WorkerState {
		Unavailable = 0,
		Connected = 1,
		Ready = 2,
		Busy = 3,
	}
	public class Worker {
		public string Identity { get; init; }
		public WorkerState State { get; set; }
		public DateTime LastHeartbeat { get; set; }
		public bool IsActive => State == WorkerState.Ready || State == WorkerState.Connected || State == WorkerState.Busy;
		public bool IsNotBusy => State == WorkerState.Ready || State == WorkerState.Connected;
		public Queue<ClientRequest> Requests { get; init; }

		public readonly static IReadOnlySet<(WorkerState, WorkerState)> ValidStateChanges;
		static Worker() {
			var set = new HashSet<(WorkerState, WorkerState)> {
				(WorkerState.Unavailable, WorkerState.Connected),
				(WorkerState.Connected, WorkerState.Unavailable),
				(WorkerState.Connected, WorkerState.Ready),
				(WorkerState.Ready, WorkerState.Busy),
				(WorkerState.Busy, WorkerState.Ready),
				(WorkerState.Ready, WorkerState.Unavailable),
				(WorkerState.Busy, WorkerState.Unavailable)
			};
			ValidStateChanges = set;
		}

		public Worker(string identity) {
			Identity = identity;
			Requests = new Queue<ClientRequest>();
		}

		public bool StateChange(WorkerState newState) {
			var change = (State, newState);
			if (ValidStateChanges.Contains(change)) {
				State = newState;
				return true;
			} else {
				return false;
			}
		}
		public BrokerRequest Accept(ClientRequest command) => new BrokerRequest(Identity, command.Id, command.Route, command.Payload);
	}
}
