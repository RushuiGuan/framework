using System.Collections.Generic;

namespace Albatross.Excel.Table {
	public record ReadTableResult<T> {
		public int FirstRow { get; set; }	// 1
		public int LastRow { get; set; }	// 0
		public int FirstColumn { get; set; }
		public int LastColumn { get; set; }
		public bool HasData => LastRow >= FirstRow;
		public int TotalRowCount => LastRow - FirstRow + 1;	// 0 -1 + 1
		public int TotalErrorRowCount { get; set; }
		public int TotalValidRowCount => TotalRowCount - TotalErrorRowCount;
		public List<T> Data { get; set; } = new List<T>();
		public List<ReadError> Errors { get; set; } = new List<ReadError>();
	}
}