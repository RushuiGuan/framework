using System.Reflection;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Albatross.CodeGen.Core {
	public static class TextWriterExtension {
		#region standard
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
        public static TextWriter WriteItems<T>(this TextWriter writer, IEnumerable<T> items, string delimiter, Action<TextWriter, T> action) {
            if (items != null) {
                int count = 0, total = items.Count();
                foreach (var item in items) {
                    action?.Invoke(writer, item);
                    count++;
                    if (count != total) {
                        writer.Append(delimiter);
                    }
                }
            }
            return writer;
        }
		#endregion

		#region C# code generation
		public static TextWriter This(this TextWriter writer, string name) {
			writer.Write("this.");
			writer.Write(name);
			return writer;
		}

		public static TextWriter Const(this TextWriter writer) {
			writer.Write("const ");
			return writer;
		}

		public static TextWriter Static(this TextWriter writer) {
			writer.Write("static ");
			return writer;
		}

		public static TextWriter ReadOnly(this TextWriter writer) {
			writer.Write("readonly ");
			return writer;
		}

		public static TextWriter Assignment(this TextWriter writer) {
			writer.Write(" = ");
			return writer;
		}

		public static TextWriter Region(this TextWriter writer, string name) {
			writer.Write("#region ");
			writer.WriteLine(name);
			return writer;
		}
		public static TextWriter EndRegion(this TextWriter writer) {
			writer.WriteLine("#endregion ");
			return writer;
		}

		public static TextWriter Literal(this TextWriter writer, object value) {
			if (value == null) {
				writer.Write("null");
			} else if (value is string) {
				writer.Write("\"");
				writer.Write(value);
				writer.Write("\"");
			} else {
				writer.Write(value);
			}
			return writer;
		}

		public static TextWriter AsString(this TextWriter writer) {
			writer.Write(".ToString()");
			return writer;
		}
		#endregion
	}
}
