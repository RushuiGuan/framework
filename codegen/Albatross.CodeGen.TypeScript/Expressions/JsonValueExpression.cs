using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class JsonValueExpression : SyntaxNode, IExpression {
		public JsonValueExpression(params JsonPropertyExpression[] properties) {
			Properties = new ListOfSyntaxNodes<JsonPropertyExpression>(properties) {
				Padding = " "
			};
		}
		public JsonValueExpression(IEnumerable<JsonPropertyExpression> properties) : this(properties.ToArray()) { }

		public ListOfSyntaxNodes<JsonPropertyExpression> Properties { get; }
		public override TextWriter Generate(TextWriter writer) {
			writer.Append("{").Code(Properties).Append("}");
			return writer;
		}
		public override IEnumerable<ISyntaxNode> Children => [Properties];
	}
}