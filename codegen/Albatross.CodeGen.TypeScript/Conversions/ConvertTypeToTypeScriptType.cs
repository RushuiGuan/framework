using Albatross.CodeGen.TypeScript.Models;
using Albatross.CodeGen.TypeScript.TypeConversions;
using System;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertTypeToTypeScriptType : IConvertObject<Type, TypeExpression> {
		private readonly TypeConverterFactory factory;
		private readonly SyntaxTree syntaxTree;

		public ConvertTypeToTypeScriptType(TypeConverterFactory factory, SyntaxTree syntaxTree) {
			this.factory = factory;
			this.syntaxTree = syntaxTree;
		}

		public TypeExpression Convert(Type type) =>  factory.Convert(this.syntaxTree, type);
		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}
