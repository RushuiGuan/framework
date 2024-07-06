using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;
using System.Text.Json;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class AnyTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(Type type) => type == typeof(object) || type == typeof(JsonElement);
		public ITypeExpression Convert(Type type, TypeConverterFactory _) => Defined.Types.Any;
	}

	public class AnyTypeConverter2 : ITypeConverter2 {
		public int Precedence => 0;
		public bool Match(ITypeSymbol symbol) => symbol.ToDisplayString() == "System.Object" 
			|| symbol.ToDisplayString() == "System.Text.Json.JsonElement";

		public ITypeExpression Convert(ITypeSymbol symbol, TypeConverterFactory2 _) => Defined.Types.Any;
	}
}
