using System;
using System.Threading.Tasks;

namespace Albatross.Collections {
	public record class LongPosition : IPosition {
		public long Value { get; }
		public LongPosition(long value) {
			this.Value = value;
		}
	}
	public record class IntPosition : IPosition {
		public int Value { get; }
		public IntPosition(int value) {
			this.Value = value;
		}
	}
	public interface IPosition { }
	public class EndPosition : IPosition { }
	public interface ISortedData<T> where T : IComparable<T> {
		T FirstKey { get; }
		T LastKey { get; }
		bool TryReadNexKey(out T key, out IPosition priorPosition);
		void Seek(IPosition position);
		void ResetPosition();
		bool Any();
		Task ReplacedBy(ISortedData<T> changes);
		Task Prepend(ISortedData<T> changes);
		Task Append(ISortedData<T> changes);
	}
}