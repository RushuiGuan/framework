﻿using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using System;
using System.Collections;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class CollectionTypeConverter : ITypeConverter {
		public int Precedence => 80;
		public bool Match(Type type) => type.IsDerived(typeof(IEnumerable))
			&& type != typeof(string)
			&& type != typeof(byte[]);

		public Expression Convert(Type type, TypeConverterFactory factory, SyntaxTree syntaxTree) {
			TypeExpression result;
			if(type.GetCollectionElementType(out var elementType)) {
				result = factory.Convert(syntaxTree, elementType);
			} else {
				result = syntaxTree.AnyType();
			}
			return syntaxTree.ArrayType(result);
		}
	}
}
