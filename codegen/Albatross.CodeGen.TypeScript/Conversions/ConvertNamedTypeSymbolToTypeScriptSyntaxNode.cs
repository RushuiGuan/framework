using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertNamedTypeSymbolToTypeScriptSyntaxNode : IConvertObject<INamedTypeSymbol, CodeGen.Syntax.SyntaxNode> {
		private readonly ConvertEnumToTypeScriptEnum2 convertEnumToTypeScriptEnum;
		private readonly ConvertClassToTypeScriptInterface convertClassToTypeScriptInterface;

		public ConvertNamedTypeSymbolToTypeScriptSyntaxNode(ConvertEnumToTypeScriptEnum2 convertEnumToTypeScriptEnum, ConvertClassToTypeScriptInterface convertClassToTypeScriptInterface) {
			this.convertEnumToTypeScriptEnum = convertEnumToTypeScriptEnum;
			this.convertClassToTypeScriptInterface = convertClassToTypeScriptInterface;
		}

		public CodeGen.Syntax.SyntaxNode Convert(INamedTypeSymbol from) {
			switch(from.TypeKind) {
				case TypeKind.Enum:
					return convertEnumToTypeScriptEnum.Convert(from);
				case TypeKind.Class:
					return convertClassToTypeScriptInterface.Convert(from);
				default:
					throw new System.NotSupportedException();
			}
		}

		object IConvertObject<INamedTypeSymbol>.Convert(INamedTypeSymbol from) {
			return Convert(from);
		}
	}
}
