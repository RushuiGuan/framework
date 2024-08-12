using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis {
	public class ArgumentListBuilder {
		List<ExpressionSyntax> expressions = new List<ExpressionSyntax>();

		public ArgumentListBuilder Add(ExpressionSyntax expression) {
			expressions.Add(expression);
			return this;
		}
		public ArgumentListSyntax Build() => SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(expressions.Select(x => SyntaxFactory.Argument(x))));
	}
}
