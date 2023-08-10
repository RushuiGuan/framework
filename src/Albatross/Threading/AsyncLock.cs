using System;
using System.Threading;

namespace Albatross.Threading {
	/// <summary>
	/// bad idea, causes deadlock, should not use
	/// </summary>
	[Obsolete]
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
