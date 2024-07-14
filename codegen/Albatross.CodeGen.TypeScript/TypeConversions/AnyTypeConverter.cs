using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;
using System.Text.Json;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class AnyTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(ITypeSymbol symbol) => symbol.ToDisplayString() == "System.Object" 
			|| symbol.ToDisplayString() == "System.Text.Json.JsonElement";

		public ITypeExpression Convert(ITypeSymbol symbol, ITypeConverterFactory _) => Defined.Types.Any;
	}
}
