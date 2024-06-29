using Albatross.CodeGen.TypeScript.Models;
using System;
using System.Text.Json;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class AnyTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(Type type) => type == typeof(object) || type == typeof(JsonElement);
		public TypeExpression Convert(Type type, TypeConverterFactory _, SyntaxTree syntaxTree) => syntaxTree.Type("any");
	}
}
