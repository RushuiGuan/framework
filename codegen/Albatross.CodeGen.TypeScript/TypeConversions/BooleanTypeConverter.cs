using Albatross.CodeGen.TypeScript.Expressions;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class BooleanTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(Type type) => type == typeof(bool);
		public ITypeExpression Convert(Type type, TypeConverterFactory _) => Defined.Types.Boolean;
	}
}
