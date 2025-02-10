using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.Text.Table {
	public static class StringTableExtensions {
		public static StringTable StringTable<T>(this IEnumerable<T> items, TableOptions<T>? options = null) {
			options = options ?? TableOptionFactory.Instance.Get<T>();
			StringTable table = new StringTable(options.ColumnOptions.Select(x => x.Header));
			foreach (var item in items) {
				table.Add(options.GetValue(item));
			}
			return table;
		}

		public static string Print(this StringTable table, bool showHeader = true) {
			var writer = new StringWriter();
			table.Print(writer, showHeader);
			return writer.ToString();
		}
	}
}
