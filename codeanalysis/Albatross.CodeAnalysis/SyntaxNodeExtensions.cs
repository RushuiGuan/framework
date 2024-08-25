using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeAnalysis {
	public static class SyntaxNodeExtensions {
		public static TypeSyntax Type(this string typeName) => SyntaxFactory.ParseTypeName(typeName);
	}
}
