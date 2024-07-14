using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class DateTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(ITypeSymbol symbol) => symbol.ToDisplayString() == "System.DateOnly"
			|| symbol.ToDisplayString() == "System.DateTime" 
			|| symbol.ToDisplayString() == "System.DateTimeOffset";

		public ITypeExpression Convert(ITypeSymbol type, TypeConverterFactory _) => Defined.Types.Date;
	}
}
