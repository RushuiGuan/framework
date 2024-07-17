using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.Modifiers;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Declarations {
	public record class MethodDeclaration : SyntaxNode, IDeclaration, ICodeElement {
		public MethodDeclaration(string name) {
			Identifier = new IdentifierNameExpression(name);
		}
		public IdentifierNameExpression Identifier { get; }
		public ITypeExpression ReturnType { get; init; } = Defined.Types.Any();
		public ListOfSyntaxNodes<ParameterDeclaration> Parameters { get; init; } = new();
		public IEnumerable<IModifier> Modifiers { get; init; } = [];
		public IExpression Body { get; init; } = new EmptyExpression();

		public override IEnumerable<ISyntaxNode> Children => new List<ISyntaxNode> { Identifier, ReturnType, Parameters, Body };

		public override TextWriter Generate(TextWriter writer) {
			var modifier = Modifiers.Where(x => x is AccessModifier).FirstOrDefault() ?? AccessModifier.Public;
			if (!object.Equals(modifier, AccessModifier.Public)) {
				writer.Append(modifier.Name).Space();
			}
			if (Modifiers.Where(x => x is AsyncModifier).Any()) {
				writer.Append("async").Space();
			}
			writer.Code(Identifier).OpenParenthesis().Code(Parameters).CloseParenthesis();
			if (!object.Equals(this.ReturnType, Defined.Types.Void())) {
				writer.Append(": ").Code(ReturnType).Space();
			}
			using (var scope = writer.BeginScope()) {
				scope.Writer.Code(Body);
			}
			writer.WriteLine();
			return writer;
		}
	}
}