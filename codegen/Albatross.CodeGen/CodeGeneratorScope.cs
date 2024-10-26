using Albatross.Text;
using System;
using System.IO;
using System.Text;

namespace Albatross.CodeGen {
	public class CodeGeneratorScope : IDisposable {
		TextWriter parentWriter;
		StringBuilder content = new StringBuilder();
		Action<TextWriter> end;
		public TextWriter Writer { get; private set; }

		public CodeGeneratorScope(TextWriter writer, Action<TextWriter> begin, Action<TextWriter> end) {
			parentWriter = writer;
			Writer = new StringWriter(content);
			begin(writer);
			this.end = end;
		}

		public void Dispose() {
			Writer.Flush();
			StringReader reader = new StringReader(content.ToString());
			string? line = reader.ReadLine();
			while (line != null) {
				parentWriter.Tab().WriteLine(line);
				line = reader.ReadLine();
			}
			end(parentWriter);
		}
	}
}