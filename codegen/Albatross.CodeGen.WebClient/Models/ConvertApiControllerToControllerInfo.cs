using Albatross.CodeGen.WebClient.CSharp;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.WebClient.Models {
	public class ConvertApiControllerToControllerInfo : IConvertObject<INamedTypeSymbol, ControllerInfo> {
		private readonly CSharpProxyControllerSettings settings;
		private readonly Compilation compilation;

		public ConvertApiControllerToControllerInfo(CSharpProxyControllerSettings settings, Compilation compilation) {
			this.settings = settings;
			this.compilation = compilation;
		}
		public ControllerInfo Convert(INamedTypeSymbol controllerSymbol) {
			return new ControllerInfo(settings, compilation, controllerSymbol);
		}
		object IConvertObject<INamedTypeSymbol>.Convert(INamedTypeSymbol from) => Convert(from);
	}
}
