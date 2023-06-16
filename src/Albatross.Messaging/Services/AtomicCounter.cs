using System.Numerics;

namespace Albatross.Messaging.Services {
	public class AtomicCounter<T> where T:INumber<T>{
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
	}
}
