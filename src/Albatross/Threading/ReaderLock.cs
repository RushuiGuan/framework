using System;
using System.Threading;

namespace Albatross.Threading {
	/// <summary>
	/// most of the time, Monitor (lock keyword) is faster than both read and write lock aquisition of ReaderWriterLockSlim.  Only use this when we have
	/// lots of read and some write and the write operation takes a long time.
	/// Always use a monitor when lock a shared resource such as increase a counter.
	/// </summary>
	public sealed class ReaderLock : IDisposable {
		private readonly ReaderWriterLockSlim readerWriterLock;
		private readonly bool canUpgrade;

		public ReaderLock(ReaderWriterLockSlim readerWriterLock, bool canUpgrade) {
			if (canUpgrade) {
				readerWriterLock.EnterUpgradeableReadLock();
			} else {
				readerWriterLock.EnterReadLock();
			}
			this.readerWriterLock = readerWriterLock;
			this.canUpgrade = canUpgrade;
		}

		public WriterLock Upgrade() => new WriterLock(readerWriterLock);
		public void Dispose() {
			if (canUpgrade) {
				readerWriterLock.ExitUpgradeableReadLock();
			} else {
				readerWriterLock.ExitReadLock();
			}
		}
	}
}
