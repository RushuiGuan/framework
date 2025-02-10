using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.Text.Table {
	public static class Extensions {
		public static TableOptions<T> Register<T>(this TableOptionFactory factory, TableOptionBuilder<T> builder) {
			var options = new TableOptions<T>(builder);
			factory.Register(options);
			return options;
		}


		public static void Table<T>(this IEnumerable<T> items, TextWriter writer, bool showHeader = true, TableOptions<T>? options = null) {
			options = options ?? TableOptionFactory.Instance.Get<T>();
			var table = items.StringTable(options);
			table.Print(writer, showHeader);
		}

		public static void MarkdownTable<T>(this IEnumerable<T> items, TextWriter writer, TableOptions<T>? options = null) {
			options = options ?? TableOptionFactory.Instance.Get<T>();
			writer.WriteItems(options.Headers, "|").WriteLine();
			writer.WriteItems(options.ColumnOptions.Select(x => "-").ToArray(), "|").WriteLine();
			foreach (var item in items) {
				writer.WriteItems(options.GetValue(item), "|").WriteLine();
			}
		}
	}
}
