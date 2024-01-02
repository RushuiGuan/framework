using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.Python {
	public static class Extensions {
		public static CodeGeneratorScope BeginPythonScope(this TextWriter writer, string? text = null) {
			return new CodeGeneratorScope(writer, args => args.AppendLine($"{text}:"), args => { });
		}
	}
}

