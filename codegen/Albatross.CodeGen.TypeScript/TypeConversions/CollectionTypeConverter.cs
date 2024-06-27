using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using System;
using System.Collections;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class CollectionTypeConverter : ITypeConverter {
		public int Precedence => 80;
		public bool Match(Type type) => type.IsDerived(typeof(IEnumerable))
			&& type != typeof(string)
			&& type != typeof(byte[]);

		public TypeScriptType Convert(Type type, TypeConverterFactory factory) {
			TypeScriptType result;
			if(type.GetCollectionElementType(out var elementType)) {
				result = factory.Convert(elementType);
			} else {
				result = TypeScriptType.Any();
			}
			result.IsArray = true;
			return result;
		}
	
	}
}
