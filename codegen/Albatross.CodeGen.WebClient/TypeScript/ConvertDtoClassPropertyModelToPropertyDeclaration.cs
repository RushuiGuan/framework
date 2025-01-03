using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.WebClient.Models;
using Albatross.Text;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.WebClient.TypeScript {
	public class ConvertDtoClassPropertyModelToPropertyDeclaration : IConvertObject<DtoClassPropertyInfo, PropertyDeclaration> {
		private readonly IConvertObject<ITypeSymbol, ITypeExpression> typeConverter;

		public ConvertDtoClassPropertyModelToPropertyDeclaration(IConvertObject<ITypeSymbol, ITypeExpression> typeConverter) {
			this.typeConverter = typeConverter;
		}

		public PropertyDeclaration Convert(DtoClassPropertyInfo from) {
			return new PropertyDeclaration(from.Name.CamelCase()) {
				Type = typeConverter.Convert(from.PropertyType),
				Optional = from.PropertyType.IsNullable(),
			};
		}

		object IConvertObject<DtoClassPropertyInfo>.Convert(DtoClassPropertyInfo from) => Convert(from);
	}
}