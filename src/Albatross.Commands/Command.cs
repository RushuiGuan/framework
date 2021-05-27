using System;

namespace Albatross.Commands {
	public abstract class Command{
		System.Threading.ManualResetEventSlim? syncHandle { get; init; }
		public Guid Id { get; init; }
		public Exception? Exception { get; private set; }
		public bool BlockUntilCompletion { get; init; }

		public Command(bool blockUntilCompletion):this(blockUntilCompletion, Guid.NewGuid()) { }
		public Command(bool blockUntilCompletion, Guid id) {
			this.Id = id;
			this.BlockUntilCompletion = blockUntilCompletion;
			if (blockUntilCompletion) {
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
