using System;
using System.Threading;

namespace Albatross.Threading {
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
