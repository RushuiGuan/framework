using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Declarations {
	public record class SetterDeclaration : MethodDeclaration, ICodeElement {
		public SetterDeclaration(string name, ITypeExpression propertyType) : base(name) {
			base.Parameters = new ListOfSyntaxNodes<ParameterDeclaration>(
				new ParameterDeclaration("value") {
					Type = propertyType,
				});
		}

		public override TextWriter Generate(TextWriter writer) {
			writer.Append("set ");
			return base.Generate(writer);
		}
	}
}