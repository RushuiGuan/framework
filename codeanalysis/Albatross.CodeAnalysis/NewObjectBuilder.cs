using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis {
	/// <summary>
	/// Create an <see cref="ObjectCreationExpressionSyntax"/>.  
	/// It expects an optional <see cref="ArgumentListSyntax"/> and zero or more optional parameters of <see cref="AssignmentExpressionSyntax"/>
	/// <see cref="ArgumentListSyntax"/> is used to create the object constructor parameters and <see cref="AssignmentExpressionSyntax"/> is used to create
	/// the property initializations
	/// <see cref="AssignmentExpressionSyntax"/> nodes can be created using <see cref="AssignmentExpressionBuilder"/> builder.
	/// </summary>
	public class NewObjectBuilder : INodeBuilder {
		public NewObjectBuilder(string typeName) {
			TypeName = typeName;
		}

		public string TypeName { get; }

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var typeSyntax = SyntaxFactory.ParseTypeName(TypeName);
			var argumentList = elements.OfType<ArgumentListSyntax>().FirstOrDefault();
			var assignments = elements.OfType<AssignmentExpressionSyntax>().ToArray();
			if (argumentList == null && assignments.Length == 0) {
				argumentList = SyntaxFactory.ArgumentList();
			}
			InitializerExpressionSyntax initializer = null;
			if (assignments.Length > 0) {
				var list = SyntaxFactory.SeparatedList(assignments.Cast<ExpressionSyntax>());
				initializer = SyntaxFactory.InitializerExpression(SyntaxKind.ObjectInitializerExpression, list);
			}
			return SyntaxFactory.ObjectCreationExpression(typeSyntax, argumentList, initializer);
		}
	}

}
