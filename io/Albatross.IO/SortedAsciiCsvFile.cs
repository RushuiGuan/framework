using Albatross.Collections;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.IO {
	public class SortedAsciiCsvFile<T> : ISortedData<T> where T : IComparable<T> {
		private readonly Func<string, T> getKey;
		private readonly Stream stream;

		public SortedAsciiCsvFile(Stream stream, Func<string, T> getKey) {
			this.getKey = getKey;
			this.stream = stream;
			if (Any()) {
				if (stream.TryReadLine(out var line, out _)) {
					this.FirstKey = getKey(line);
				} else {
					throw new ArgumentException();
				}
				if (stream.TryReadLastLine(out line)) {
					this.LastKey = getKey(line);
				} else {
					throw new ArgumentException();
				}
				if (FirstKey.CompareTo(LastKey) > 0) {
					throw new ArgumentException();
				}
			} else {
				this.FirstKey = default!;
				this.LastKey = default!;
			}
		}

		public T FirstKey { get; }

		public T LastKey { get; }

		public bool Any() => stream.Length > 0;

		public async Task Append(ISortedData<T> changes) {
			if (changes is SortedAsciiCsvFile<T> file) {
				await stream.Append(file.stream);
			} else {
				throw new ArgumentException();
			}
		}

		public async Task Prepend(ISortedData<T> changes) {
			if (changes is SortedAsciiCsvFile<T> file) {
				await stream.Prepend(file.stream, false);
			} else {
				throw new ArgumentException();
			}
		}

		public async Task ReplacedBy(ISortedData<T> changes) {
			if (changes is SortedAsciiCsvFile<T> file) {
				stream.Seek(0, SeekOrigin.Begin);
				file.stream.Seek(0, SeekOrigin.Begin);
				await file.stream.CopyToAsync(stream);
				stream.SetLength(file.stream.Length);
			} else {
				throw new ArgumentException();
			}
		}

		public void ResetPosition() {
			stream.Seek(0, SeekOrigin.Begin);
		}

		public void Seek(IPosition position) {
			if (position is EndPosition) {
				stream.Seek(0, SeekOrigin.End);
			} else if (position is LongPosition pos) {
				stream.Seek(pos.Value, SeekOrigin.Begin);
			} else {
				throw new ArgumentException();
			}
		}

		public bool TryReadNexKey(out T key, out IPosition priorPosition) {
			if (stream.TryReadLine(out var line, out long priorPos)) {
				key = getKey(line);
				priorPosition = new LongPosition(priorPos);
				return true;
			} else {
				key = default!;
				priorPosition = new LongPosition(0);
				return false;
			}
		}
	}
}
