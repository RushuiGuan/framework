using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class BooleanTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(Type type) => type == typeof(bool);
		public ITypeExpression Convert(Type type, TypeConverterFactory _) => Defined.Types.Boolean;
	}

	public class BooleanTypeConverter2 : ITypeConverter2 {
		public int Precedence => 0;
		public bool Match(ITypeSymbol symbol) => symbol.ToDisplayString() == "System.Boolean";
		public ITypeExpression Convert(ITypeSymbol symbol, TypeConverterFactory2 _) => Defined.Types.Boolean;
	}
}
