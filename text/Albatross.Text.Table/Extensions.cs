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

		/// <summary>
		/// Print the data as a formatted table to the TextWriter.  
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="writer"></param>
		/// <param name="showHeader"></param>
		/// <param name="options"></param>
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
		
		public static void Console<T>(this IEnumerable<T> items, bool showHeader = true, TableOptions<T>? options = null) {
			options = options ?? TableOptionFactory.Instance.Get<T>();
			var table = items.StringTable(options);
			var width = System.Console.BufferWidth;
			table.Print(System.Console.Out, showHeader, width);
		}
	}
}
