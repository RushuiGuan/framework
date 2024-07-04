using Albatross.CodeGen.TypeScript.Expressions;
using System;
using System.Text.Json;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class AnyTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(Type type) => type == typeof(object) || type == typeof(JsonElement);
		public ITypeExpression Convert(Type type, TypeConverterFactory _) => Defined.Types.Any;
	}
}
