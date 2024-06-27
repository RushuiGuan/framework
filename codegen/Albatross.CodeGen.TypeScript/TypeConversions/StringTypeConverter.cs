using Albatross.CodeGen.TypeScript.Models;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class StringTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(Type type) => type == typeof(string) 
			|| type == typeof(char) 
			|| type == typeof(TimeSpan) 
			|| type == typeof(byte[]);

		public TypeScriptType Convert(Type type, TypeConverterFactory _) => TypeScriptType.String();
	}
}
