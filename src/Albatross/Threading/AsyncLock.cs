using System;
using System.Threading;

namespace Albatross.Threading {
	public sealed class AsyncLock: IDisposable {
		private readonly SemaphoreSlim semaphore;

		public AsyncLock(SemaphoreSlim semaphore) {
			this.semaphore = semaphore;
			this.semaphore.Wait();
		}

		public void Dispose() {
			this.semaphore.Release();
		}
	}
}
