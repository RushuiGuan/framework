using Albatross.CodeGen.TypeScript.Models;
using System;
using System.Text.Json;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class AnyTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(Type type) => type == typeof(object) || type == typeof(JsonElement);
		public TypeScriptType Convert(Type type, TypeConverterFactory _) => TypeScriptType.Any();
	}
}
