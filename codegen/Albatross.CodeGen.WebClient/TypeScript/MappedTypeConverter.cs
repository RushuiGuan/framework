using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.TypeConversions;
using Albatross.CodeGen.WebClient.Settings;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.WebClient.TypeScript {
	public class MappedTypeConverter : ITypeConverter {
		private TypeScriptWebClientSettings settings;

		public MappedTypeConverter(CodeGenSettings settings) {
			this.settings = settings.TypeScriptWebClientSettings;
		}
		// this should have higher precedence than CustomTypeConversion
		public int Precedence => 998;

		public bool TryConvert(ITypeSymbol symbol, IConvertObject<ITypeSymbol, ITypeExpression> factory, [NotNullWhen(true)] out ITypeExpression? expression) {
			if (settings.TypeMapping.TryGetValue(symbol.GetFullName(), out var mappedType)) {
				expression = new SimpleTypeExpression {
					 Identifier = mappedType.ParseIdentifierName(),
				};
				return true;
			} else {
				expression = null;
				return false;
			}
		}
	}
}
