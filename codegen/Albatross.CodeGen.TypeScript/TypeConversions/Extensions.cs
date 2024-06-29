using Albatross.CodeGen.TypeScript.Models;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public static class Extensions {
		public static TypeExpression Convert(this TypeConverterFactory factory, SyntaxTree tree, Type type) {
			if (factory.TryGet(type, out var converter)) {
				return converter.Convert(type, factory, tree);
			} else {
				return tree.Type(type.Name);
			}
		}
	}
}
