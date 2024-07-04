using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.TypeConversions;
using Albatross.Reflection;
using System;
using System.Reflection;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertParameterInfoToParameterDeclaration : IConvertObject<ParameterInfo, ParameterDeclaration> {
		private readonly ConvertTypeToTypeScriptType typeConverter;
		private readonly TypeConverterFactory factory;

		public ConvertParameterInfoToParameterDeclaration(ConvertTypeToTypeScriptType typeConverter, TypeConverterFactory factory) {
			this.typeConverter = typeConverter;
			this.factory = factory;
		}

		public ParameterDeclaration Convert(ParameterInfo info) {
			return new ParameterDeclaration() {
				Identifier = new IdentifierNameExpression(info.Name ?? throw new Exception()),
				Type = factory.Convert(info.ParameterType),
				Optional = info.ParameterType.IsNullableValueType(),
			};
		}

		object IConvertObject<ParameterInfo>.Convert(ParameterInfo from) => this.Convert(from);
	}
}
