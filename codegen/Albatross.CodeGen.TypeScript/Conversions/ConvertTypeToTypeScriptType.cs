using Albatross.CodeGen.TypeScript.Models;
using Albatross.CodeGen.TypeScript.TypeConversions;
using System;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertTypeToTypeScriptType : IConvertObject<Type, TypeExpression> {
		private readonly TypeConverterFactory factory;

		public ConvertTypeToTypeScriptType(TypeConverterFactory factory) {
			this.factory = factory;
		}

		public TypeExpression Convert(Type type) =>  factory.Convert(type);
		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}
