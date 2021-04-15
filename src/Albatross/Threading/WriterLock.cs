using System;
using System.Threading;

namespace Albatross.Threading {
	public sealed class WriterLock : IDisposable {
		private readonly ReaderWriterLockSlim readerWriterLock;

		public WriterLock(ReaderWriterLockSlim readerWriterLock) {
			readerWriterLock.EnterWriteLock();
			this.readerWriterLock = readerWriterLock;
		}
		public void Dispose() => readerWriterLock.ExitWriteLock();
	}
}
