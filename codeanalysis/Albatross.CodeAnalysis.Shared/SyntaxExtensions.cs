using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeAnalysis {
	public static class SyntaxExtensions {
		public static NamespaceDeclarationSyntax NamespaceDeclaration(this string @namespace)
				=> SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(@namespace)).NormalizeWhitespace();
	
		public static TypeSyntax TypeSyntax(this string type) => SyntaxFactory.ParseTypeName(type);


	}
}