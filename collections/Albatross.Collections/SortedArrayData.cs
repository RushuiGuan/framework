using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Collections {
	public class SortedArrayData<T, R> : ISortedData<T> where T : IComparable<T> where R : notnull {
		public SortedArrayData(IEnumerable<R> items, Func<R, T> getKey) {
			if (items.Any()) {
				FirstKey = getKey(items.First());
				LastKey = getKey(items.Last());
			} else {
				FirstKey = default!;
				LastKey = default!;
			}
			this.items = items.ToArray();
			this.getKey = getKey;
		}

		private R[] items;
		private int position;
		private readonly Func<R, T> getKey;

		public bool Any() => items.Any();
		public R[] Items => items;
		public T FirstKey { get; }
		public T LastKey { get; }
		public T GetKey(R record) => this.getKey(record);
		public bool TryReadNexKey(out T key, out IPosition positionPrior) {
			positionPrior = new IntPosition(this.position);
			if (items.Length > this.position) {
				key = GetKey(this.items[position]);
				position++;
				return true;
			} else {
				key = default!;

				return false;
			}
		}

		public R[] ReadRemaining() {
			var result = new R[items.Length - position];
			Array.Copy(items, position, result, 0, result.Length);
			position = items.Length;
			return result;
		}

		public void ResetPosition() {
			this.position = 0;
		}

		public Task ReplacedBy(ISortedData<T> changes) {
			if (changes is SortedArrayData<T, R> typed) {
				this.items = typed.items;
				ResetPosition();
			} else {
				throw new ArgumentException();
			}
			return Task.CompletedTask;
		}

		public Task Prepend(ISortedData<T> changes) {
			if (changes is SortedArrayData<T, R> typed) {
				var array = typed.ReadRemaining();
				this.items = array.Union(this.items).ToArray();
				ResetPosition();
			} else {
				throw new ArgumentException();
			}
			return Task.CompletedTask;
		}

		public Task Append(ISortedData<T> changes) {
			if (changes is SortedArrayData<T, R> typed) {
				var changes_array = typed.ReadRemaining();
				var newArray = new R[this.position + changes_array.Length];
				if (this.position > 0) {
					Array.Copy(this.items, 0, newArray, 0, this.position);
				}
				Array.Copy(changes_array, 0, newArray, this.position, changes_array.Length);
				this.items = newArray;
				ResetPosition();
			} else {
				throw new ArgumentException();
			}
			return Task.CompletedTask;
		}

		public void Seek(IPosition position) {
			if (position is IntPosition intPos) {
				if (intPos.Value >= 0 && intPos.Value < this.items.Length) {
					this.position = intPos.Value;
				} else {
					throw new IndexOutOfRangeException();
				}
			} else if (position is EndPosition) {
				this.position = this.items.Length;
			} else {
				throw new ArgumentException();
			}
		}
	}
}