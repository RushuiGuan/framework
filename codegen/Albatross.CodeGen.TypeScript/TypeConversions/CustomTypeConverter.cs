using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {

	public class CustomTypeConverter : ITypeConverter {
		private readonly ISourceLookup sourceLookup;
		private readonly ILogger<CustomTypeConverter> logger;

		public CustomTypeConverter(ISourceLookup sourceLookup, ILogger<CustomTypeConverter> logger) {
			this.sourceLookup = sourceLookup;
			this.logger = logger;
		}

		public int Precedence => 999;

		public bool TryConvert(ITypeSymbol symbol, IConvertObject<ITypeSymbol, ITypeExpression> factory, [NotNullWhen(true)] out ITypeExpression? expression) {
			if (sourceLookup.TryGet(symbol, out ISourceExpression source)) {
				logger.LogInformation("Found source {source} for symbol {symbol}", source.ToString(), symbol.GetFullName());
				expression = new SimpleTypeExpression { Identifier = new QualifiedIdentifierNameExpression(symbol.Name, source) };
			} else {
				expression = new SimpleTypeExpression { Identifier = new IdentifierNameExpression(symbol.Name) };
				logger.LogInformation("No module source was found for symbol {symbol}", symbol.GetFullName());
			}
			return true;
		}
	}
}