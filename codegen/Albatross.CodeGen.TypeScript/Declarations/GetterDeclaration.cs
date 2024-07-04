using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Declarations {
	public record class GetterDeclaration : MethodDeclaration, ICodeElement {
		public GetterDeclaration(string name) : base(name) {
			Parameters = new Syntax.ListOfSyntaxNodes<ParameterDeclaration>();
		}

		public override TextWriter Generate(TextWriter writer) {
			writer.Append("get ");
			return base.Generate(writer);
		}
	}
}
