namespace Albatross.CodeGen.WebClient.Settings {
	public record class CodeGenSettings {
		public TypeScriptWebClientSettings TypeScriptWebClientSettings { get; init; } = new TypeScriptWebClientSettings();
		public CSharpWebClientSettings CSharpWebClientSettings { get; init; } = new CSharpWebClientSettings();
		public ApiControllerConversionSettings ApiControllerConversionSetting { get; init; } = new ApiControllerConversionSettings();

		public SymbolFilterPatterns ControllerFilter { get; init; } = new SymbolFilterPatterns();
		public SymbolFilterPatterns DtoFilter{ get; init; } = new SymbolFilterPatterns();
		public SymbolFilterPatterns EnumFilter { get; init; } = new SymbolFilterPatterns();


		public SymbolFilterPatterns CSharpControllerFilter => this.CSharpWebClientSettings.ControllerFilter.Overwrite(this.ControllerFilter);
		public SymbolFilterPatterns TypeScriptControllerFilter => this.TypeScriptWebClientSettings.ControllerFilter.Overwrite(this.ControllerFilter);
		public SymbolFilterPatterns TypeScriptDtoFilter => this.TypeScriptWebClientSettings.DtoFilter.Overwrite(this.DtoFilter);
		public SymbolFilterPatterns TypeScriptEnumFilter => this.TypeScriptWebClientSettings.EnumFilter.Overwrite(this.EnumFilter);
	}
}
