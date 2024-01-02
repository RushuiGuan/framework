using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Method : ICodeElement{
		public bool IsStatic { get; set; }
		public string Name { get; set; }
		public ICodeElement CodeBlock { get; set; } = new CodeBlock(new Pass());
		public ParameterCollection Parameters { get; set; } = new ParameterCollection(Enumerable.Empty<Parameter>());
		public Method(string name) {
			Name = name;
		}

		public TextWriter Generate(TextWriter writer) {
			if (IsStatic) {
				writer.AppendLine("@staticmethod");
			} else {
				Parameters.InsertSelfWhenMissing();
			}
			writer.Append("def ").Append(Name).OpenParenthesis().Code(Parameters).CloseParenthesis();
			using(var scope = writer.BeginPythonScope()) {
				scope.Writer.Code(CodeBlock);
			}
			writer.WriteLine();
			return writer;
		}
	}
	public class Constructor : Method {
		public Constructor() : base("__init__") {
		}
	}
}