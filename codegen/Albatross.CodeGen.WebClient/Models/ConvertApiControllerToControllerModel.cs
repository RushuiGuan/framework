using Albatross.CodeGen.WebClient.Settings;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.WebClient.Models {
	public class ConvertApiControllerToControllerModel : IConvertObject<INamedTypeSymbol, ControllerInfo> {
		private readonly CodeGenSettings settings;
		private readonly Compilation compilation;

		public ConvertApiControllerToControllerModel(CodeGenSettings settings, Compilation compilation) {
			this.settings = settings;
			this.compilation = compilation;
		}
		public ControllerInfo Convert(INamedTypeSymbol controllerSymbol) {
			return new ControllerInfo(settings.CSharpWebClientSettings, compilation, controllerSymbol);
		}
		object IConvertObject<INamedTypeSymbol>.Convert(INamedTypeSymbol from) => Convert(from);
	}
}