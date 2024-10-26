
namespace Albatross.Messaging.Services {
	public interface IAtomicCounter {
		ulong NextId();
		ulong Counter { get; }
	}
	public class AtomicCounter : IAtomicCounter {
		object sync = new object();
		ulong counter = 0;

		public AtomicCounter(ulong counter) {
			this.counter = counter;
		}

		public ulong NextId() {
			lock (sync) {
				counter = counter + 1;
				return counter;
			}
		}
		public ulong Counter => this.counter;
	}
}