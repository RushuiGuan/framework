using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.WebClient.Models {
	public class ConvertApiControllerToWebApi : IConvertObject<INamedTypeSymbol, WebApi> {
		private readonly Compilation compilation;

		public ConvertApiControllerToWebApi(Compilation compilation) {
			this.compilation = compilation;
		}
		public WebApi Convert(INamedTypeSymbol controllerSymbol) {
			return new WebApi(compilation, controllerSymbol);
		}
		object IConvertObject<INamedTypeSymbol>.Convert(INamedTypeSymbol from) => Convert(from);
	}
}
