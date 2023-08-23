using System.Numerics;

namespace Albatross.Messaging.Services {
	public interface IAtomicCounter<T> where T : INumber<T> {
		T NextId();
		T Counter {get;}
	}
	public class AtomicCounter<T> : IAtomicCounter<T> where T : INumber<T> {
		object sync = new object();
		T counter = T.Zero;
		public AtomicCounter(T counter) {
			this.counter = counter;
		}

		public T NextId() {
			lock (sync) {
				counter = counter + T.One;
				return counter;
			}
		}
		public T Counter => this.counter;
	}
}
