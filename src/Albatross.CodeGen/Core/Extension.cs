using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.Core {
	public static class Extension {
        public static CodeGeneratorScope BeginScope(this TextWriter writer, string? text = null) {
            return new CodeGeneratorScope(writer, args => args.AppendLine($"{text} {{"), args => args.Append("}"));
        }

		public static TextWriter Code(this TextWriter writer, ICodeElement codeElement) {
			codeElement.Generate(writer);
			return writer;
		}
	}
}