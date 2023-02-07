using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Albatross.Reflection;

namespace Albatross.Text {
	public static partial class TextWriterExtension {
		public static TextWriter Append(this TextWriter writer, object obj) {
			writer.Write(obj);
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

		public static void PrintProperties<T>(this TextWriter writer, T? data, params string[] properties) 
			=> writer.PrintProperties<T>(new T?[] { data }, new PrintPropertiesOption(), properties);

		public static void PrintProperties<T>(this TextWriter writer, T?[] items, PrintPropertiesOption option, params string[] properties) {
			int columnCount = items.Length + 1;
			int[] columnWidth = new int[columnCount];
			List<string?[]> rows = new List<string?[]>();
			string?[] row;
			if (option?.GetHeader != null) {
				row = new string[columnCount];
				row[0] = null;
				for (int i = 1; i < columnCount; i++) {
					row[i] = option.GetHeader(i - 1);
					columnWidth[i] = System.Math.Max(row[i]?.Length ?? 0, columnWidth[i]);
				}
				rows.Add(row);
			}
			Type type = typeof(T);
			foreach (var name in properties) {
				row = new string[columnCount];
				rows.Add(row);
				row[0] = name;
				columnWidth[0] = System.Math.Max(columnWidth[0], name.Length);
				for (int i = 0; i < items.Length; i++) {
					var value = type.GetPropertyValue(items[i], name);
					if (option?.FormatValue != null) {
						row[i + 1] = option.FormatValue(name, value);
					} else {
						row[i + 1] = Convert.ToString(value);
					}
					columnWidth[i + 1] = System.Math.Max(columnWidth[i + 1], row[i + 1]?.Length ?? 0);
				}
			}
			bool headerSeperator = false;
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
				if(option?.HasHeaderSeperator == true && !headerSeperator) {
					headerSeperator = true;
					writer.AppendChar(option.HeaderSeperator, columnWidth.Sum(args => args) + columnWidth.Length - 1).WriteLine();
				}
			}
		}
	}
}
