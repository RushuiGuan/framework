﻿using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.Reflection;
using System;
using System.Collections;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class CollectionTypeConverter : ITypeConverter {
		public int Precedence => 80;
		public bool Match(Type type) => type.IsDerived(typeof(IEnumerable))
			&& type != typeof(string)
			&& type != typeof(byte[]);

		public ITypeExpression Convert(Type type, TypeConverterFactory factory) {
			ITypeExpression result;
			if (type.GetCollectionElementType(out var elementType)) {
				result = factory.Convert(elementType);
			} else {
				result = Defined.Types.Any;
			}
			return new ArrayTypeExpression {
				Type = result,
			};
		}
	}
}
