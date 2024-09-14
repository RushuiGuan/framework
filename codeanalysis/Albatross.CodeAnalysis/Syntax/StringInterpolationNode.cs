using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Create a <see cref="InterpolationSyntax"/> instance />.  Expect one or 2 string literals.  The first one is the text
	/// the second one is the format string.
	/// </summary>
	public class StringInterpolationNode : NodeContainer {
		public StringInterpolationNode(string text, string? format) : base(Build(text, format)) {
		}
		static InterpolationSyntax Build(string text, string? format) {
			var result = SyntaxFactory.Interpolation(SyntaxFactory.IdentifierName(text));
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
