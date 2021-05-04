using System;
using System.Collections.Generic;

namespace Albatross.Commands {
	public abstract class Command{
		System.Threading.ManualResetEventSlim? syncHandle { get; init; }
		public Guid Id { get; init; }
		public Exception? Exception { get; private set; }
		public bool BlockUntilCompletion { get; init; }

		public Command(bool waitForCompletion) {
			Id = Guid.NewGuid();
			this.BlockUntilCompletion = waitForCompletion;
			if (waitForCompletion) {
				syncHandle = new System.Threading.ManualResetEventSlim(false);
			}
		}
		public void Error(Exception err) {
			this.Exception = err;
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
