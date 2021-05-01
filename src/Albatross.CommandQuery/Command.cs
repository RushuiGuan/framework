using System;
using System.Collections.Generic;

namespace Albatross.CommandQuery {
	public abstract class Command{
		System.Threading.ManualResetEventSlim? syncHandle { get; init; }
		public Guid Id { get; init; }
		public Exception? Error { get; private set; }
		public abstract string QueueName { get; }

		public Command(bool synchronized) {
			Id = Guid.NewGuid();
			if (synchronized) {
				syncHandle = new System.Threading.ManualResetEventSlim(false);
			}
		}
		public void Fail(Exception err) {
			this.Error = err;
		}

		public void Wait() {
			this.syncHandle?.Wait();
		}
		public void Complete() {
			this.syncHandle?.Set();
			this.syncHandle?.Dispose();
		}
	}
}
