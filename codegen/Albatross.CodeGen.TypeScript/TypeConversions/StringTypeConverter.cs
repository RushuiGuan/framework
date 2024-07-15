using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class StringTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(ITypeSymbol symbol) => symbol.GetFullName() == "System.String"
			|| symbol.GetFullName() == "System.Char"
			|| symbol.GetFullName() == "System.Char[]"
			|| symbol.GetFullName() == "System.Guid"
			|| symbol.GetFullName() == "System.Byte[]";
		public ITypeExpression Convert(ITypeSymbol symbol, ITypeConverterFactory _) => Defined.Types.String;
	}
}
