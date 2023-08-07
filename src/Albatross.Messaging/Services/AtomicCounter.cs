using System.Numerics;

namespace Albatross.Messaging.Services {
	public interface IAtomicCounter<T> where T : INumber<T> {
		T NextId();
		void Set(T value);
		T Counter {get;}
	}
	public class AtomicCounter<T> : IAtomicCounter<T> where T : INumber<T> {
		object sync = new object();
		T counter = T.Zero;
		public T NextId() {
			lock (sync) {
				counter = counter + T.One;
				return counter;
			}
		}
		public void Set(T value) {
			lock (sync) {
				counter = value;
			}
		}
		public T Counter => this.counter;
	}
}
