using Albatross.Text;

namespace Albatross.Text.Table {
	public static class Extensions {
		public static StringTable StringTable<T>(this IEnumerable<T> items, TableOptions<T>? options = null) {
			options = options ?? TableOptionFactory.Instance.Get<T>();
			StringTable table = new StringTable(options.ColumnHeaders);
			foreach (var item in items) {
				table.Add(options.GetValue(item));
			}
			return table;
		}

		public static void Write(this StringTable table, TextWriter writer, bool showHeader = true) {
			if(showHeader) {
				foreach (var column in table.Columns) {
					writer.Write(column.Name.PadRight(column.MaxWidth + 1));
				}
				writer.WriteLine();
			}
			foreach(var row in table.Rows) {
				for(int i=0; i < row.Values.Length; i++) {
					var column = table.Columns[i];
					var text = row.Values[i];
					if(text.Length > column.MaxWidth) {
 						text = text.Substring(0, column.MaxWidth);
					}
					if (column.AlignRight) {
						text = text.PadLeft(column.MaxWidth + 1);
					} else {
						text = text.PadRight(column.MaxWidth + 1);
					}
					writer.Write(text);
				}
				writer.WriteLine();
			}
		}
	}
}
