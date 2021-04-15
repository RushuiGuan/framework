using System.Threading;

namespace Albatross.Threading {
	public static class Extension {
		public static ReaderLock GetReadLock(this ReaderWriterLockSlim readerWriterLock) => new ReaderLock(readerWriterLock, false);
		public static ReaderLock GetUpgradeableReadLock(this ReaderWriterLockSlim readerWriterLock) => new ReaderLock(readerWriterLock, true);
		public static WriterLock GetWriteLock(this ReaderWriterLockSlim readerWriterLock) => new WriterLock(readerWriterLock);
		public static AsyncLock EnterAsyncLock(this SemaphoreSlim semaphore) {
			return new AsyncLock(semaphore);
		}
	}
}
