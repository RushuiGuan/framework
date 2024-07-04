using Albatross.CodeGen.TypeScript.Expressions;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class VoidTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(Type type) => type == typeof(void);
		public ITypeExpression Convert(Type type, TypeConverterFactory _) => Defined.Types.Void;
	}
}
