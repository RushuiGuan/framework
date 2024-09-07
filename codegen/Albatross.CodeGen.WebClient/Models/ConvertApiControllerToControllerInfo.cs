using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.WebClient.Models {
	public class ConvertApiControllerToControllerInfo : IConvertObject<INamedTypeSymbol, ControllerInfo> {
		private readonly Compilation compilation;

		public ConvertApiControllerToControllerInfo(Compilation compilation) {
			this.compilation = compilation;
		}
		public ControllerInfo Convert(INamedTypeSymbol controllerSymbol) {
			return new ControllerInfo(compilation, controllerSymbol);
		}
		object IConvertObject<INamedTypeSymbol>.Convert(INamedTypeSymbol from) => Convert(from);
	}
}
