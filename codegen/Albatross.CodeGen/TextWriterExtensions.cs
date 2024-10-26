using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen {
	public static class TextWriterExtensions {
		public static TextWriter Code(this TextWriter writer, ICodeElement codeElement) {
			codeElement.Generate(writer);
			return writer;
		}

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

		#region typescript generation
		public static TextWriter StringLiteral(this TextWriter writer, string text, bool singleQuote = true) {
			char quoteCharacter = singleQuote ? '\'' : '"';
			writer.Append(quoteCharacter);
			foreach (char c in text) {
				if (c == quoteCharacter) {
					writer.Append('\\');
				}
				writer.Append(c);
			}
			writer.Append(quoteCharacter);
			return writer;

		}
		#endregion
	}
}