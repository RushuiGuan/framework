using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class ImportCollection : ListOfSyntaxNodes<ImportExpression> {
		public ImportCollection(IEnumerable<ImportExpression> imports) : base(imports.GroupBy(x => x.Source)
			.Select(x => new ImportExpression(x.SelectMany(y => y.Items)) {
				Source = x.Key,
			})
		) { }

		public ImportCollection(IEnumerable<ISyntaxNode> nodes) : base(
			nodes.Where(x => x is QualifiedIdentifierNameExpression)
				.Cast<QualifiedIdentifierNameExpression>()
				.GroupBy(x => x.Source)
				.Select(x => new ImportExpression(x.Select(y => y.Identifier)) {
					Source = x.Key,
				})) {
		}
		public override TextWriter Generate(TextWriter writer) {
			var sorted = this.OrderBy(x => x.Source.ToString()).ToArray();
			writer.WriteItems(sorted, "", (writer, t) => writer.Code(t), null, null);
			return writer;
		}
	}
}