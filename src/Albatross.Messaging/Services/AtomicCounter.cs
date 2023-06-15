using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Messaging.Services {
	public class AtomicCounter {
		object sync = new object();
		ulong counter = 0;
		public ulong NextId() {
			lock (sync) {
				return ++counter;
			}
		}
	}
}
