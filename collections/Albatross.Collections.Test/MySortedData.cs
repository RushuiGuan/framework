using System;
using System.Threading.Tasks;

namespace Albatross.Collections.Test {
	public class MySortedData : ISortedData<int> {
		public MySortedData(int firstkey, int lastKey) {
			if (firstkey > lastKey) {
				throw new ArgumentException();
			}
			this.FirstKey = firstkey;
			this.LastKey = lastKey;
		}
		public int FirstKey { get; }
		public int LastKey { get; }

		public bool Any() {
			throw new NotImplementedException();
		}

		public Task Append(ISortedData<int> changes) {
			throw new NotImplementedException();
		}

		public int GetKey(int record) => record;

		public Task Prepend(ISortedData<int> changes) {
			throw new NotImplementedException();
		}

		public Task ReplacedBy(ISortedData<int> changes) {
			throw new NotImplementedException();
		}

		public void ResetPosition() {
			throw new NotImplementedException();
		}

		public void Seek(IPosition position) {
			throw new NotImplementedException();
		}

		public bool TryReadNexKey(out int key, out IPosition positionPrior) {
			throw new NotImplementedException();
		}
	}
}