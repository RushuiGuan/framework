using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	public class GenericIdentifierNode : NodeContainer {
		public GenericIdentifierNode(string name, params string[] genericTypes) : base(CreateGenericType(name, genericTypes)) { }
		public GenericNameSyntax Identifier => (GenericNameSyntax)Node;
		static GenericNameSyntax CreateGenericType(string typeName, IEnumerable<string> genericTypeArguments) {
			var arguments = SyntaxFactory.SeparatedList(genericTypeArguments.Select(x => SyntaxFactory.ParseTypeName(x)));
			return SyntaxFactory.GenericName(SyntaxFactory.Identifier(typeName), SyntaxFactory.TypeArgumentList(arguments));
		}
	}
}
