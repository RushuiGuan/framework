using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Albatross.Reflection;
using System.Threading.Tasks;

namespace Albatross.Text {
	public static partial class PrintTextExtensions {
		public static Task PrintProperties<T>(this TextWriter writer, T? data, PrintPropertiesOption option)
			=> writer.PrintProperties<T>(new T?[] { data }, option);

		public static async Task PrintProperties<T>(this TextWriter writer, T?[] items, PrintPropertiesOption option) {
			int columnCount = items.Length + 1;
			int[] columnWidth = new int[columnCount];
			var rows = new List<string?[]>();
			string?[] row;
			if (option.GetColumnHeader != null) {
				row = new string[columnCount];
				row[0] = null;
				for (int i = 1; i < columnCount; i++) {
					row[i] = option.GetColumnHeader(i - 1);
					columnWidth[i] = System.Math.Max(row[i]?.Length ?? 0, columnWidth[i]);
				}
				rows.Add(row);
			}
			Type type = typeof(T);
			foreach (var name in option.Properties) {
				row = new string[columnCount];
				rows.Add(row);
				row[0] = option.GetRowHeader?.Invoke(name) ?? name;
				columnWidth[0] = System.Math.Max(columnWidth[0], name.Length);
				for (int i = 0; i < items.Length; i++) {
					var value = type.GetPropertyValue(items[i], name, true);
					if (option.FormatValue != null) {
						row[i + 1] = await option.FormatValue(items[i], name, value);
					} else {
						row[i + 1] = Convert.ToString(value);
					}
					columnWidth[i + 1] = System.Math.Max(columnWidth[i + 1], row[i + 1]?.Length ?? 0);
				}
			}
			bool columnHeaderLineDrawn = false;
			foreach (var r in rows) {
				for (int i = 0; i < columnCount; i++) {
					if (i == 0) {
						writer.Write((r[i] ?? string.Empty).PadLeft(columnWidth[i]));
					} else {
						writer.Append((r[i] ?? string.Empty).PadRight(columnWidth[i]));
					}
					if (i == columnCount - 1) {
						writer.WriteLine();
					} else {
						writer.Space();
					}
				}
				if (option.HasColumnHeaderLine && !columnHeaderLineDrawn) {
					columnHeaderLineDrawn = true;
					writer.AppendChar(option.ColumnHeaderLineCharacter, columnWidth.Sum(args => args) + columnWidth.Length - 1).WriteLine();
				}
			}
		}

		public static async Task PrintTable<T>(this TextWriter writer, T?[] items, PrintTableOption option) {
			bool columnHeaderLine = false;
			int columnCount = option.Properties.Length;
			int[] columnWidth = new int[columnCount];
			List<string?[]> rows = new List<string?[]>();
			string?[] row;

			if (option.PrintHeader) {
				row = new string[columnCount];
				rows.Add(row);
				for (int i = 0; i < columnCount; i++) {
					row[i] = option.GetColumnHeader(option.Properties[i]);
					columnWidth[i] = System.Math.Max(row[i]?.Length ?? 0, columnWidth[i]);
				}
			} else {
				columnHeaderLine = true;
			}
			Type type = typeof(T);
			foreach (var item in items) {
				row = new string[columnCount];
				rows.Add(row);
				for (int i = 0; i < columnCount; i++) {
					var value = type.GetPropertyValue(item, option.Properties[i], true);
					row[i] = await option.FormatValue(item, option.Properties[i], value);
					columnWidth[i] = System.Math.Max(row[i]?.Length ?? 0, columnWidth[i]);
				}
			}

			foreach (var r in rows) {
				for (int i = 0; i < columnCount; i++) {
					writer.Append((r[i] ?? string.Empty).PadRight(columnWidth[i]));
					if (i == columnCount - 1) {
						writer.WriteLine();
					} else {
						writer.Space();
					}
				}
				if (!columnHeaderLine) {
					columnHeaderLine = true;
					writer.AppendChar(option.ColumnHeaderLineCharacter, columnWidth.Sum(args => args) + columnWidth.Length - 1).WriteLine();
				}
			}
		}

		public static void PrintSideBySide(this TextWriter writer, string leftSideText, string rightSideText, char seperator = ' ', int seperatorWidth = 1) {
			int maxWidth = 0;
			var leftSideReader = new StringReader(leftSideText);
			var rightSideReader = new StringReader(rightSideText);

			List<string> leftSideList = new List<string>();
			for (string? line = leftSideReader.ReadLine(); line != null; line = leftSideReader.ReadLine()) {
				leftSideList.Add(line);
				maxWidth = line.Length > maxWidth ? line.Length : maxWidth;
			}
			foreach (var line in leftSideList) {
				writer.Append(line.PadRight(maxWidth));
				writer.AppendChar(seperator, seperatorWidth);
				writer.AppendLine(rightSideReader.ReadLine() ?? string.Empty);
			}
			for (string? line = rightSideReader.ReadLine(); line != null; line = rightSideReader.ReadLine()) {
				writer.Space(maxWidth).AppendChar(seperator, seperatorWidth).AppendLine(line);
			}
		}
	
		public static void Indent(this TextWriter writer, string text, char letter = ' ', int seperatorWidth = 4)
			=> writer.PrintSideBySide("", text, letter, seperatorWidth);
	}
}
