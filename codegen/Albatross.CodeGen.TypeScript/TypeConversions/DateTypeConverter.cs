using Albatross.CodeGen.TypeScript.Models;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class DateTypeConverter: ITypeConverter {
		public int Precedence => 0;
		public TypeScriptType Convert(Type type, TypeConverterFactory _) => TypeScriptType.Date();
		public bool Match(Type type) => type == typeof(DateOnly) || type == typeof(DateTime) || type == typeof(TimeOnly);
	}
}
