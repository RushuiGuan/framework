using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.Text.Table {
	public class StringTable {
		public class Column {
			public Column(string name, int index) {
				Name = name;
				Index = index;
				SetMaxWidth(name.Length);
			}
			public int Index { get; }
			public string Name { get; }
			/// <summary>
			/// The maximum width of the data for this column
			/// </summary>
			public int MaxWidth { get; private set; }

			public void SetMaxWidth(int width) {
				if (width > MaxWidth) {
					MaxWidth = width;
					DisplayWidth = width;
				}
			}
			/// <summary>
			/// The minimum width of this column.  If set to 0, the column will be hidden
			/// </summary>
			public int MinWidth { get; set; } = int.MaxValue;
			public int DisplayWidth { get; internal set; }
			public int ActualWidth => DisplayWidth == 0 ? 0 : DisplayWidth + 1;
			public bool AlignRight { get; set; }
			public string GetText(string value) {
				if (value.Length > DisplayWidth) {
					value = value.Substring(0, DisplayWidth);
				}
				if (AlignRight) {
					value = value.PadLeft(DisplayWidth);
				} else {
					value = value.PadRight(DisplayWidth);
				}
				return value;
			}
		}
		public class Row {
			public string[] Values { get; }
			public Row(params string[] values) {
				Values = values;
			}
		}
		Column[] columns;
		List<Row> rows = new List<Row>();

		public Column[] Columns => columns;
		public IEnumerable<Row> Rows => rows;

		public StringTable(params IEnumerable<string> headers) {
			columns = headers.Select((x, index) => new Column(x, index)).ToArray();
		}

		public int TotalWidth => Columns.Sum(x => x.ActualWidth) - 1;

		public void ResetColumns() {
			foreach (var column in Columns) {
				column.DisplayWidth = column.MaxWidth;
			}
		}

		public void AdjustColumnWidth(int maxWidth) {
			var overflow = TotalWidth - maxWidth;
			if (overflow > 0) {
				for (int i = Columns.Length - 1; i >= 0; i--) {
					var column = Columns[i];
					// if the MinWidth of the column is set, reduce the DisplayWidth as much as possible
					if (column.MinWidth < column.MaxWidth) {
						column.DisplayWidth = Math.Max(column.MinWidth, column.DisplayWidth - overflow);
						overflow = TotalWidth - maxWidth;
						// when a column width is set to 0, the reduced width could be width + 1 therefore
						// could lead to a negative overflow
						if (overflow <= 0) {
							return;
						}
					}
				}
				for (int i = columns.Length - 1; i >= 0; i--) {
					var column = Columns[i];
					// this time, reduce the DisplayWidth regardless of the MinWidth
					column.DisplayWidth = Math.Max(0, column.DisplayWidth - overflow);
					overflow = TotalWidth - maxWidth;
					if (overflow <= 0) {
						return;
					}
				}
			}
		}

		public void Add(params IEnumerable<string> values) {
			var array = values.ToArray();
			if (array.Length != columns.Length) {
				throw new ArgumentException($"Table is expecting rows with {columns.Length} columns");
			}
			rows.Add(new Row(array));
			for (int i = 0; i < columns.Length; i++) {
				columns[i].SetMaxWidth(array[i].Length);
			}
		}

		public void Print(TextWriter writer) {
			var visibleColumns = this.Columns.Where(x => x.DisplayWidth > 0).ToArray();
			var line = string.Join(" ", visibleColumns.Select(x => x.GetText(x.Name)));
			writer.WriteLine(line);
			writer.WriteLine("-".PadRight(line.Length, '-'));
			foreach (var row in Rows) {
				line = string.Join(" ", visibleColumns.Select(x => x.GetText(row.Values[x.Index])));
				writer.WriteLine(line);
			}
			if (!string.IsNullOrEmpty(line)) {
				writer.WriteLine("-".PadRight(line.Length, '-'));
			}
		}
	}
}