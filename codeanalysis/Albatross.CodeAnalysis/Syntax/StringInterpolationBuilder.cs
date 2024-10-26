using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Create a <see cref="InterpolationSyntax"/> instance />/>
	/// </summary>
	public class StringInterpolationBuilder : INodeBuilder {
		private readonly string? format;
		IdentifierNameSyntax? identifier;

		public StringInterpolationBuilder() { }
		public StringInterpolationBuilder(string format) {
			this.format = format;
		}
		public StringInterpolationBuilder(IdentifierNode identifier, string format) {
			this.identifier = identifier.Identifier;
			this.format = format;
		}
		public StringInterpolationBuilder(string identifier, string format) : this(new IdentifierNode(identifier), format) { }

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var elementList = new List<SyntaxNode>(elements);
			if (this.identifier != null) {
				elementList.Add(this.identifier);
			}
			var expressionSyntax = (ExpressionSyntax)new MemberAccessBuilder().Build(elementList);
			var result = SyntaxFactory.Interpolation(expressionSyntax);
			if (!string.IsNullOrEmpty(format)) {
				result = result.WithFormatClause(
					SyntaxFactory.InterpolationFormatClause(
						SyntaxFactory.Token(SyntaxKind.ColonToken),
						SyntaxFactory.Token(SyntaxTriviaList.Empty, SyntaxKind.InterpolatedStringTextToken, format!, format!, SyntaxTriviaList.Empty)
					)
				);
			}
			return result;
		}
	}
}