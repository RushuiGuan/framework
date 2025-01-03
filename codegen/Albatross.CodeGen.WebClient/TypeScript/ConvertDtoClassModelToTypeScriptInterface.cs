using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript;
using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.WebClient.Models;
using Albatross.CodeGen.WebClient.Settings;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Albatross.CodeGen.WebClient.TypeScript {
	public class ConvertDtoClassModelToTypeScriptInterface : IConvertObject<DtoClassInfo, InterfaceDeclaration> {
		private readonly CodeGenSettings settings;
		private readonly IConvertObject<DtoClassPropertyInfo, PropertyDeclaration> propertyConverter;

		public ConvertDtoClassModelToTypeScriptInterface(CodeGenSettings settings, IConvertObject<DtoClassPropertyInfo, PropertyDeclaration> propertyConverter) {
			this.settings = settings;
			this.propertyConverter = propertyConverter;
		}

		public InterfaceDeclaration Convert(DtoClassInfo from) {
			ITypeExpression? baseInterfaceName = null;
			foreach (var baseTypeName in from.BaseTypes) {
				if (settings.TypeScriptWebClientSettings.BaseTypeMapping.TryGetValue(baseTypeName, out var mappedType)) {
					baseInterfaceName = new SimpleTypeExpression {
						Identifier = mappedType.ParseIdentifierName(),
					};
				}
			}
			var declaration = new InterfaceDeclaration(from.Name) {
				Properties = from.Properties.Select(x => propertyConverter.Convert(x)).ToList(),
				BaseInterfaceName = baseInterfaceName,
			};
			return declaration;
		}

		object IConvertObject<DtoClassInfo>.Convert(DtoClassInfo from) => Convert(from);
	}
}