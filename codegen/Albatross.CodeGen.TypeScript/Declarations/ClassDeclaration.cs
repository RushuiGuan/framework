using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.Collections;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Declarations {
	public record class ClassDeclaration : SyntaxNode, IDeclaration, ICodeElement {
		public ClassDeclaration(string name) {
			this.Identifier = new IdentifierNameExpression(name);
		}
		public IdentifierNameExpression Identifier { get; }
		public IIdentifierNameExpression? BaseClassName { get; init; }
		public InvocationExpression[] Decorators { get; init; } = [];
		public MethodDeclaration? Constructor { get; init; }
		public IEnumerable<IModifier> Modifiers { get; init; } = [];
		public IEnumerable<ImportExpression> Imports { get; init; } = [];
		public IEnumerable<GetterDeclaration> Getters { get; init; } = [];
		public IEnumerable<SetterDeclaration> Setters { get; init; } = [];
		public IEnumerable<PropertyDeclaration> Properties { get; init; } = [];
		public IEnumerable<MethodDeclaration> Methods { get; init; } = [];

		public override IEnumerable<ISyntaxNode> Children
			=> new List<ISyntaxNode> { Identifier, }
					.AddIfNotNull(BaseClassName)
					.AddIfNotNull(Constructor)
					.UnionAll(Decorators, Imports, Getters, Setters, Properties, Methods);

		public override TextWriter Generate(TextWriter writer) {
			Decorators.ForEach(x => writer.Code(x).AppendLine());
			writer.Append("export ").Append("class ").Code(Identifier);
			if (BaseClassName != null) {
				writer.Append(" extends ").Code(BaseClassName);
			}
			using (var scope = writer.BeginScope()) {
				foreach (var getter in Getters) {
					scope.Writer.Code(getter);
				}
				foreach (var setter in Setters) {
					scope.Writer.Code(setter);
				}
				foreach (var property in Properties) {
					scope.Writer.Code(property);
				}
				if (Constructor != null) {
					scope.Writer.Code(Constructor);
				}
				foreach (var method in Methods) {
					scope.Writer.Code(method);
				}
			}
			writer.WriteLine();
			return writer;
		}
	}
}