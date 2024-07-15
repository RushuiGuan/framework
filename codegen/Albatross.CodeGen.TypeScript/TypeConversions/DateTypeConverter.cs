using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class DateTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(ITypeSymbol symbol) => symbol.GetFullName() == "System.DateOnly"
			|| symbol.GetFullName() == "System.DateTime"
			|| symbol.GetFullName() == "System.TimeSpan"
			|| symbol.GetFullName() == "System.DateTimeOffset";

		public ITypeExpression Convert(ITypeSymbol type, ITypeConverterFactory _) => Defined.Types.Date;
	}
}
