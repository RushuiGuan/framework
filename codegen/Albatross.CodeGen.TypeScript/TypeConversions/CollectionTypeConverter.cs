using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.Reflection;
using Microsoft.CodeAnalysis;
using System;
using System.Collections;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class CollectionTypeConverter : ITypeConverter {
		private readonly Compilation compilation;

		public CollectionTypeConverter(Compilation compilation){
			this.compilation = compilation;
		}

		public int Precedence => 80;
		public bool Match(ITypeSymbol symbol) => symbol is IArrayTypeSymbol arrayTypeSymbol && symbol.ToDisplayString() != "System.Byte[]"
			|| symbol.IsDerivedFrom(compilation.GetTypeByMetadataName("System.Collections.IEnumerable"));

		public ITypeExpression Convert(ITypeSymbol symbol, TypeConverterFactory factory) {
			ITypeExpression result;
			if (symbol is IArrayTypeSymbol arrayTypeSymbol) {
				result = factory.Convert(arrayTypeSymbol.ElementType);
			} else {
				result = Defined.Types.Any;
			}
			return new ArrayTypeExpression {
				Type = result,
			};
		}
	}

}
