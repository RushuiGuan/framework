﻿using Albatross.CodeGen.WebClient.Settings;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.WebClient.Models {
	public class ConvertApiControllerToControllerInfo : IConvertObject<INamedTypeSymbol, ControllerInfo> {
		private readonly CodeGenSettings settings;
		private readonly Compilation compilation;

		public ConvertApiControllerToControllerInfo(CodeGenSettings settings, Compilation compilation) {
			this.settings = settings;
			this.compilation = compilation;
		}
		public ControllerInfo Convert(INamedTypeSymbol controllerSymbol) {
			return new ControllerInfo(settings.ApiControllerConversionSetting, compilation, controllerSymbol);
		}
		object IConvertObject<INamedTypeSymbol>.Convert(INamedTypeSymbol from) => Convert(from);
	}
}