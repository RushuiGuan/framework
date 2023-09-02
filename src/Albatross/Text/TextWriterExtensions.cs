using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Albatross.Reflection;
using System.Threading.Tasks;

namespace Albatross.Text {
	public static partial class TextWriterExtensions {
		public static TextWriter Append(this TextWriter writer, object obj) {
			writer.Write(obj);
			return writer;
		}
		public static TextWriter AppendBooleanAsBit(this TextWriter writer, bool value) {
			if (value) {
				writer.Write(1);
			} else {
				writer.Write(0);
			}
			return writer;
		}
		public static TextWriter AppendLine(this TextWriter writer, object obj) {
			writer.WriteLine(obj);
			return writer;
		}
		public static TextWriter AppendChar(this TextWriter writer, char c, int count = 1) {
			for (int i = 0; i < count; i++) {
				writer.Write(c);
			}
			return writer;
		}
		public static TextWriter Tab(this TextWriter writer, int count = 1) {
			return writer.AppendChar('\t', count);
		}
		public static TextWriter Dot(this TextWriter writer, int count = 1) {
			return writer.AppendChar('.', count);
		}
		public static TextWriter Comma(this TextWriter writer, int count = 1) {
			return writer.AppendChar(',', count);
		}
		public static TextWriter OpenSquareBracket(this TextWriter writer, int count = 1) {
			return writer.AppendChar('[', count);
		}
		public static TextWriter CloseSquareBracket(this TextWriter writer, int count = 1) {
			return writer.AppendChar(']', count);
		}
		public static TextWriter OpenAngleBracket(this TextWriter writer, int count = 1) {
			return writer.AppendChar('<', count);
		}
		public static TextWriter CloseAngleBracket(this TextWriter writer, int count = 1) {
			return writer.AppendChar('>', count);
		}
		public static TextWriter OpenParenthesis(this TextWriter writer, int count = 1) {
			return writer.AppendChar('(', count);
		}
		public static TextWriter CloseParenthesis(this TextWriter writer, int count = 1) {
			return writer.AppendChar(')', count);
		}
		public static TextWriter Space(this TextWriter writer, int count = 1) {
			return writer.AppendChar(' ', count);
		}
		public static TextWriter Semicolon(this TextWriter writer, int count = 1) {
			return writer.AppendChar(';', count);
		}
		/// <summary>
		/// String.Join for TextWriter
		/// </summary>
		/// <returns>Current text writer</returns>
		public static TextWriter WriteItems<T>(this TextWriter writer, IEnumerable<T> items, string delimiter, Action<TextWriter, T>? action = null) {
			int count = 0, total = items.Count();
			foreach (var item in items) {
				if (item != null) {
					if (action == null) {
						writer.Append(item);
					} else {
						action.Invoke(writer, item);
					}
					count++;
					if (count != total) {
						writer.Append(delimiter);
					}
				} else {
					total--;
				}
			}
			return writer;
		}

		public static Task PrintProperties<T>(this TextWriter writer, T? data, params string[] properties)
			=> writer.PrintProperties<T>(new T?[] { data }, new PrintPropertiesOption(properties));

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
					var value = type.GetPropertyValue(items[i], name);
					if (option.FormatValue != null) {
						try {
							row[i + 1] = await option.FormatValue(name, value);
						} catch {
							row[i + 1] = "Format Error";
						}
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
			int columnCount = option.Properties.Length;
			int[] columnWidth = new int[columnCount];
			List<string?[]> rows = new List<string?[]>();
			string?[] row;
			row = new string[columnCount];
			rows.Add(row);
			for (int i = 0; i < columnCount; i++) {
				row[i] = option.GetColumnHeader(option.Properties[i]);
				columnWidth[i] = System.Math.Max(row[i]?.Length ?? 0, columnWidth[i]);
			}
			Type type = typeof(T);
			foreach (var item in items) {
				row = new string[columnCount];
				rows.Add(row);
				for (int i = 0; i < columnCount; i++) {
					var value = type.GetPropertyValue(item, option.Properties[i]);
					try {
						row[i] = await option.FormatValue(option.Properties[i], value);
					} catch {
						row[i] = "Format Error";
					}
					columnWidth[i] = System.Math.Max(row[i]?.Length ?? 0, columnWidth[i]);
				}
			}

			bool columnHeaderLine = false;
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
	}
}
