using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.Reflection;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class NumericTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(Type type) => type.IsNumericType();
		public ITypeExpression Convert(Type type, TypeConverterFactory _)
			=> Defined.Types.Numeric;
	}
}
