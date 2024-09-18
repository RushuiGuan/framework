using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient.Settings {
	public record class ApiControllerConversionSettings {
		public WebClientMethodSettings? GlobalMethodSettings { get; set; }
		public Dictionary<string, WebClientMethodSettings> MethodSettings { get; init; } = new Dictionary<string, WebClientMethodSettings>();

		public WebClientMethodSettings Get(string controllerName, string methodName) {
			var key = $"{controllerName}.{methodName}";
			var globalSettings = GlobalMethodSettings ?? new WebClientMethodSettings();
			if (MethodSettings.TryGetValue(key, out var methodSettings)) {
				return methodSettings.Overwrite(globalSettings);
			} else {
				return globalSettings;
			}
		}
	}
}
