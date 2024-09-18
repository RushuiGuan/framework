using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.WebClient.Models;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.WebClient.TypeScript {
	public class ConvertDtoClassPropertyInfoToPropertyDeclaration : IConvertObject<DtoClassPropertyInfo, PropertyDeclaration> {
		private readonly IConvertObject<ITypeSymbol, ITypeExpression> typeConverter;

		public ConvertDtoClassPropertyInfoToPropertyDeclaration(IConvertObject<ITypeSymbol, ITypeExpression> typeConverter) {
			this.typeConverter = typeConverter;
		}

		public PropertyDeclaration Convert(DtoClassPropertyInfo from) {
			return new PropertyDeclaration(from.Name) {
				Type = typeConverter.Convert(from.Type),
				Optional = from.Type.IsNullable(),
			};
		}

		object IConvertObject<DtoClassPropertyInfo>.Convert(DtoClassPropertyInfo from) => Convert(from);
	}
}
