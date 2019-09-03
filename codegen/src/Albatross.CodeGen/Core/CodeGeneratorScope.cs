using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.Core
{
    public class CodeGeneratorScope :IDisposable
    {
        TextWriter parentWriter;
        StringBuilder content = new StringBuilder();
        Action<TextWriter> end;
        public TextWriter Writer { get; private set; }

        public CodeGeneratorScope(TextWriter writer, Action<TextWriter> begin, Action<TextWriter> end) {
            this.parentWriter = writer;
            Writer = new StringWriter(content);
            begin(writer);
            this.end = end;
        }

        public void Dispose()
        {
            Writer.Flush();
            StringReader reader = new StringReader(content.ToString());
            string line = reader.ReadLine();
            while (line != null) {
				if (string.IsNullOrEmpty(line)) {
					parentWriter.WriteLine();
				} else { 
					parentWriter.Tab().WriteLine(line);
				}
                line = reader.ReadLine();
            }
            this.end(parentWriter);
        }
    }
}
