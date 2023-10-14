using ExcelDna.Integration;
using System;

namespace Albatross.Excel.Table {
	public record class TableOptions {
		public int FirstRow { get; set; }
		public int FirstColumn { get; set; }
		public int LastColumn => FirstColumn + Columns.Length - 1;
		public TableColumn[] Columns { get; set; } = Array.Empty<TableColumn>();
	}
}