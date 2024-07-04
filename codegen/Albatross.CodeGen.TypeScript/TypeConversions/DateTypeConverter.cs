using Albatross.CodeGen.TypeScript.Expressions;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class DateTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(Type type) => type == typeof(DateOnly) || type == typeof(DateTime) || type == typeof(TimeOnly) || type == typeof(DateTimeOffset);
		public ITypeExpression Convert(Type type, TypeConverterFactory _) => Defined.Types.Date;
	}
}
