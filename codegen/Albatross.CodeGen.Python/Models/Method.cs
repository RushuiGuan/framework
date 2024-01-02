using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Method : ICodeElement{
		public bool IsStatic { get; set; }
		public string Name { get; set; }
		public List<Decorator> Decorators { get; set; } = new List<Decorator>();
		public ICodeElement CodeBlock { get; set; } = new CodeBlock(new Pass());
		public ParameterCollection Parameters { get; set; } = new ParameterCollection(Enumerable.Empty<Parameter>());
		public Method(string name) {
			Name = name;
		}

		public TextWriter Generate(TextWriter writer) {
			if (IsStatic) {
				Decorators.Add(My.StaticMethod);
			} else {
				Parameters.InsertSelfWhenMissing();
			}
			foreach(var decorator in Decorators) {
				writer.Code(decorator).WriteLine();
			}
			writer.Append(My.Keywords.Self).Space().Append(Name).OpenParenthesis().Code(Parameters).CloseParenthesis();
			using(var scope = writer.BeginPythonScope()) {
				scope.Writer.Code(CodeBlock);
			}
			writer.WriteLine();
			return writer;
		}
	}
}