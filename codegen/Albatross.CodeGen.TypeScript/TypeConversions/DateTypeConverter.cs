using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class DateTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(Type type) => type == typeof(DateOnly) || type == typeof(DateTime) || type == typeof(TimeOnly) || type == typeof(DateTimeOffset);
		public ITypeExpression Convert(Type type, TypeConverterFactory _) => Defined.Types.Date;
	}

	public class DateTypeConverter2 : ITypeConverter2 {
		public int Precedence => 0;
		public bool Match(ITypeSymbol symbol) => symbol.ToDisplayString() == "System.DateOnly"
			|| symbol.ToDisplayString() == "System.DateTime" 
			|| symbol.ToDisplayString() == "System.DateTimeOffset";

		public ITypeExpression Convert(ITypeSymbol type, TypeConverterFactory2 _) => Defined.Types.Date;
	}
}
