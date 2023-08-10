using System;
using System.Threading;

namespace Albatross.Threading {
	public static class Extensions {
		public static ReaderLock GetReadLock(this ReaderWriterLockSlim readerWriterLock) => new ReaderLock(readerWriterLock, false);
		public static ReaderLock GetUpgradeableReadLock(this ReaderWriterLockSlim readerWriterLock) => new ReaderLock(readerWriterLock, true);
		public static WriterLock GetWriteLock(this ReaderWriterLockSlim readerWriterLock) => new WriterLock(readerWriterLock);
		[Obsolete]
		public static AsyncLock EnterAsyncLock(this SemaphoreSlim semaphore) {
			return new AsyncLock(semaphore);
		}
	}
}
