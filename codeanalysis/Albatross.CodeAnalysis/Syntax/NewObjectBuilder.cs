using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Create an <see cref="ObjectCreationExpressionSyntax"/>.  
	/// It expects an optional <see cref="ArgumentListSyntax"/> and zero or more optional parameters of <see cref="AssignmentExpressionSyntax"/>
	/// <see cref="ArgumentListSyntax"/> is used to create the object constructor parameters and <see cref="AssignmentExpressionSyntax"/> is used to create
	/// the property initializations
	/// <see cref="AssignmentExpressionSyntax"/> nodes can be created using <see cref="AssignmentExpressionBuilder"/> builder.
	/// </summary>
	public class NewObjectBuilder : INodeBuilder {
		private NewObjectBuilder(TypeSyntax typeSyntax) {
			Node = SyntaxFactory.ObjectCreationExpression(typeSyntax);
		}
		public NewObjectBuilder(TypeNode typeNode) : this(typeNode.Type) { }
		public NewObjectBuilder(string typeName) : this(SyntaxFactory.IdentifierName(typeName)) { }
		public ObjectCreationExpressionSyntax Node { get; private set; }

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var argumentList = elements.OfType<ArgumentListSyntax>().FirstOrDefault();
			var assignments = elements.OfType<AssignmentExpressionSyntax>().ToArray();
			if (argumentList == null) {
				if (assignments.Length == 0) {
					Node = Node.WithArgumentList(SyntaxFactory.ArgumentList());
				}
			} else {
				Node = Node.WithArgumentList(argumentList);
			}
			InitializerExpressionSyntax initializer = null;
			if (assignments.Length > 0) {
				var list = SyntaxFactory.SeparatedList(assignments.Cast<ExpressionSyntax>());
				initializer = SyntaxFactory.InitializerExpression(SyntaxKind.ObjectInitializerExpression, list);
				Node = Node.WithInitializer(initializer);
			}
			return Node;
		}
	}

}