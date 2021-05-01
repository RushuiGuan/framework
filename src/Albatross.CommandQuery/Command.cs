using System;
using System.Collections.Generic;

namespace Albatross.CommandQuery {
	public abstract class Command{
		System.Threading.ManualResetEventSlim? ManualResetEvent { get; init; }
		public Guid Id { get; init; }
		public Exception? Error { get; private set; }
		public abstract string QueueName { get; }

		public Command(bool sync) {
			Id = Guid.NewGuid();
			if (sync) {
				ManualResetEvent = new System.Threading.ManualResetEventSlim(false);
			}
		}
		public void Fail(Exception err) {
			this.Error = err;
		}

		public void Wait() {
			this.ManualResetEvent?.Wait();
		}
		public void Complete() {
			this.ManualResetEvent?.Set();
			this.ManualResetEvent?.Dispose();
		}
	}
}
