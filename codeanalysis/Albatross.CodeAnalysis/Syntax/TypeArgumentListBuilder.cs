using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeAnalysis.Syntax {
	public class TypeArgumentListBuilder : NodeContainer {
		public TypeArgumentListBuilder(params string[] typeNames) : base(Create(typeNames)) { }
		static TypeArgumentListSyntax Create(IEnumerable<string> typeNames) {
			var list = new List<TypeSyntax>();
			foreach (var typeName in typeNames) {
				list.Add(SyntaxFactory.ParseTypeName(typeName));
			}
			return SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(list));
		}
	}
}