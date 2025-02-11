using System;
using System.Collections.Generic;
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

		public static StringTable SetColumn(this StringTable table, Func<StringTable.Column, bool> predicate, Action<StringTable.Column> action) {
			foreach (var column in table.Columns.Where(x => predicate(x))) {
				action(column);
			}
			return table;
		}

		public static StringTable MinWidth(this StringTable table, Func<StringTable.Column, bool> predicate, int minWidth)
			=> table.SetColumn(predicate, x => x.MinWidth = minWidth);



		public static StringTable AlignRight(this StringTable table, Func<StringTable.Column, bool> predicate, bool value = true)
			=> table.SetColumn(predicate, x => x.AlignRight = value);

		public static StringTable ResetColumns(this StringTable table) {
			table.ResetColumns();
			return table;
		}

		public static void PrintConsole(this StringTable table) {
			table.AdjustColumnWidth(System.Console.BufferWidth);
			table.Print(System.Console.Out);
		}
	}
}
