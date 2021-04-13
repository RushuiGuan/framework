using System;
using System.Threading;

namespace Albatross.Threading {
	public static class Extension {
		public static ReaderLock GetReadLock(this ReaderWriterLockSlim readerWriterLock) => new ReaderLock(readerWriterLock, false);
		public static ReaderLock GetUpgradeableReadLock(this ReaderWriterLockSlim readerWriterLock) => new ReaderLock(readerWriterLock, true);
		public static WriterLock GetWriteLock(this ReaderWriterLockSlim readerWriterLock) => new WriterLock(readerWriterLock);
	}

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

	public sealed class WriterLock : IDisposable {
		private readonly ReaderWriterLockSlim readerWriterLock;

		public WriterLock(ReaderWriterLockSlim readerWriterLock) {
			readerWriterLock.EnterWriteLock();
			this.readerWriterLock = readerWriterLock;
		}
		public void Dispose() => readerWriterLock.ExitWriteLock();
	}
}
