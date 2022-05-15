using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Albatross.Text {
	public static class TextWriterExtension {
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
	}
}
