using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.WebClient.Models;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Albatross.CodeGen.WebClient.TypeScript {
	public class ConvertDtoClassModelToTypeScriptInterface : IConvertObject<DtoClassInfo, InterfaceDeclaration> {
		private readonly IConvertObject<DtoClassPropertyInfo, PropertyDeclaration> propertyConverter;

		public ConvertDtoClassModelToTypeScriptInterface(IConvertObject<DtoClassPropertyInfo, PropertyDeclaration> propertyConverter) {
			this.propertyConverter = propertyConverter;
		}

		public InterfaceDeclaration Convert(DtoClassInfo from) {
			var declaration = new InterfaceDeclaration(from.Name) {
				Properties = from.Properties.Select(x => propertyConverter.Convert(x)).ToList(),
			};
			return declaration;
		}
		object IConvertObject<DtoClassInfo>.Convert(DtoClassInfo from) => Convert(from);
	}
}