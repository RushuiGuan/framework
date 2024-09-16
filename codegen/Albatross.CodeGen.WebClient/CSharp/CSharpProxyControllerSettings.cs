using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient.CSharp {
	public record class CSharpProxyControllerSettings {
		public string Namespace { get; init; } = "MyNamespace";
		public CSharpProxyMethodSettings? GlobalMethodSettings { get; set; }
		public Dictionary<string, CSharpProxyMethodSettings> MethodSettings { get; init; } = new Dictionary<string, CSharpProxyMethodSettings>();

		public CSharpProxyMethodSettings Get(string controllerName, string methodName) {
			var key = $"{controllerName}.{methodName}";
			var globalSettings = GlobalMethodSettings ?? new CSharpProxyMethodSettings();
			if (MethodSettings.TryGetValue(key, out var methodSettings)) {
				return globalSettings.Overwrite(methodSettings);
			} else {
				return globalSettings;
			}
		}
	}

	public record class CSharpProxyMethodSettings {
		public bool? UseDateTimeAsDateOnly { get; set; }

		public CSharpProxyMethodSettings Overwrite(CSharpProxyMethodSettings other) {
			return new CSharpProxyMethodSettings {
				UseDateTimeAsDateOnly = other.UseDateTimeAsDateOnly ?? this.UseDateTimeAsDateOnly,
			};
		}
	}
}