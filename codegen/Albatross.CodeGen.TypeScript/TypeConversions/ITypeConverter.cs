using Albatross.CodeGen.TypeScript.Models;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public interface ITypeConverter {
		public bool Match(Type type);
		public int Precedence { get; }
		public TypeScriptType Convert(Type type, TypeConverterFactory factory);
	}
}
