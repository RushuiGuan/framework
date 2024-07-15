using Albatross.Collections;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Declarations {
	public record class InterfaceDeclaration : SyntaxNode, IDeclaration, ICodeElement {
		public InterfaceDeclaration(string name){
			this.Identifier = new IdentifierNameExpression(name);
		}
		public IdentifierNameExpression Identifier { get; }
		public ITypeExpression? BaseInterfaceName { get; init; }
		public IEnumerable<IModifier> Modifiers { get; init; } = [];
		public IEnumerable<PropertyDeclaration> Properties { get; init; } = [];

		public override IEnumerable<ISyntaxNode> Children => new List<ISyntaxNode> { Identifier, }
				.AddIfNotNull(BaseInterfaceName)
				.UnionAll(Properties);

		public override TextWriter Generate(TextWriter writer) {
			this.GetDescendants().Where(x=>x is QualifiedIdentifierNameExpression)
				.Cast<QualifiedIdentifierNameExpression>()
				.GroupBy(x=>x.Source)
				.Select(x=> new ImportExpression() {
					 Source = x.Key,
					 Items = new ListOfSyntaxNodes<IdentifierNameExpression>(x.Select(y => y.Identifier))
				}).ForEach(x=>writer.Code(x).WriteLine());

			writer.Append("export ").Append("interface ").Code(Identifier);
			if (BaseInterfaceName != null) {
				writer.Append(" extends ").Code(BaseInterfaceName);
			}
			using (var scope = writer.BeginScope()) {
				foreach (var property in Properties) {
					scope.Writer.Code(property).AppendLine(";");
				}
			}
			writer.WriteLine();
			return writer;
		}
	}
}
