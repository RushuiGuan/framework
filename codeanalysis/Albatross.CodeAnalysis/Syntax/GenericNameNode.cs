using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	public class GenericNameNode : NodeContainer {
		public GenericNameNode(string typeName, params string[] genericTypeArguments) 
			: base(CreateGenericType(typeName, genericTypeArguments)) {
		}

		static GenericNameSyntax CreateGenericType(string typeName, IEnumerable<string> genericTypeArguments) {
			var arguments = SyntaxFactory.SeparatedList(genericTypeArguments.Select(x => SyntaxFactory.ParseTypeName(x)));
			return SyntaxFactory.GenericName(SyntaxFactory.Identifier(typeName), SyntaxFactory.TypeArgumentList(arguments));
		}
	}
}
