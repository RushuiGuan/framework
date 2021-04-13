using System;
using System.Threading;

namespace Albatross.Threading {
	public static class Extension {
		public static ReaderLock EnterReadLock(this ReaderWriterLockSlim readerWriterLock, bool canUpgrade = false) {
			return new ReaderLock(readerWriterLock, canUpgrade);
		}

		public static WriterLock EnterWriteLock(this ReaderWriterLockSlim readerWriterLock) {
			return new WriterLock(readerWriterLock);
		}
	}

	public class ReaderLock : IDisposable {
		private readonly ReaderWriterLockSlim readerWriterLock;

		public ReaderLock(ReaderWriterLockSlim readerWriterLock, bool canUpgrade) {
			if (canUpgrade) {
				readerWriterLock.EnterUpgradeableReadLock();
			} else {
				readerWriterLock.EnterReadLock();
			}
			this.readerWriterLock = readerWriterLock;
		}

		public WriterLock Upgrade() => new WriterLock(readerWriterLock);
		public void Dispose() => readerWriterLock.ExitReadLock();
	}

	public class WriterLock : IDisposable {
		private readonly ReaderWriterLockSlim readerWriterLock;

		public WriterLock(ReaderWriterLockSlim readerWriterLock) {
			readerWriterLock.EnterWriteLock();
			this.readerWriterLock = readerWriterLock;
		}

		public void Dispose() => readerWriterLock.ExitWriteLock();
	}
}
