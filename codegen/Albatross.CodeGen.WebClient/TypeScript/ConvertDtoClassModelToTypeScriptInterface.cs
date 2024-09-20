using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.WebClient.Models;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Albatross.CodeGen.WebClient.TypeScript {
	public class ConvertDtoClassModelToTypeScriptInterface : IConvertObject<DtoClassInfo, InterfaceDeclaration> {
		private readonly IConvertObject<ITypeSymbol, ITypeExpression> typeConverter;
		private readonly IConvertObject<DtoClassPropertyInfo, PropertyDeclaration> propertyConverter;

		public ConvertDtoClassModelToTypeScriptInterface(IConvertObject<ITypeSymbol, ITypeExpression> typeConverter, 
			IConvertObject<DtoClassPropertyInfo, PropertyDeclaration> propertyConverter) {
			this.typeConverter = typeConverter;
			this.propertyConverter = propertyConverter;
		}

		public InterfaceDeclaration Convert(DtoClassInfo from) {
			return new InterfaceDeclaration(from.Name) {
				BaseInterfaceName = from.BaseType != null && !from.BaseType.IsValueType ? typeConverter.Convert(from.BaseType) : null,
				Properties = from.Properties.Select(x => propertyConverter.Convert(x)).ToList(),
			};
		}

		object IConvertObject<DtoClassInfo>.Convert(DtoClassInfo from) => Convert(from);
	}
}
