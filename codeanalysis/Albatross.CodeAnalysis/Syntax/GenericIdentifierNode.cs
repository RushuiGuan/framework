﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	public class GenericIdentifierNode : TypeNode {
		public GenericIdentifierNode(string name, params string[] genericTypes) : this(name, genericTypes.Select(x => new TypeNode(x)).ToArray()) { }
		public GenericIdentifierNode(string name, params TypeNode[] genericTypes) : base(CreateGenericType(name, genericTypes)) { }

		static GenericNameSyntax CreateGenericType(string typeName, IEnumerable<TypeNode> genericTypeArguments) {
			var arguments = SyntaxFactory.SeparatedList(genericTypeArguments.Select(x => x.Type));
			return SyntaxFactory.GenericName(SyntaxFactory.Identifier(typeName), SyntaxFactory.TypeArgumentList(arguments));
		}
	}

	public class ArrayTypeNode : TypeNode {
		public ArrayTypeNode(TypeNode elementType) : base(CreateCollectionType(elementType)) { }

		static ArrayTypeSyntax CreateCollectionType(TypeNode elementType) {
			return SyntaxFactory.ArrayType(elementType.Type, SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
				SyntaxFactory.ArrayRankSpecifier(
					SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
						SyntaxFactory.OmittedArraySizeExpression()
					)
				)
			));
		}
	}
}