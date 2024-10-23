using System.Collections.Generic;

namespace Albatross.IO {
	public record class SortedFileMetaData<T> where T : IComparer<T> {
		public SortedFileMetaData(T firstKey, T lastKey) {
			FirstKey = firstKey;
			LastKey = lastKey;
		}
		public int RecordCount { get; }
		public T FirstKey { get; set; }
		public T LastKey { get; set; }
	}
}
