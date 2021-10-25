using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.Core {
	/// <summary>
	/// This is created because IHostEnvironment will return production if ASPNETCORE_ENVIRONMENT OR DOTNET_ENVIRONMENT
	/// variable is not set
	/// </summary>
	public class EnvironmentSetting {
		public const string UnknownEnvironment = "Unknown";

		public string Value { get; }
		public string HostName => System.Net.Dns.GetHostName();

		public EnvironmentSetting(string variable) {
			Value = System.Environment.GetEnvironmentVariable(variable)?.ToLower()?? UnknownEnvironment;
		}

		public readonly static EnvironmentSetting ASPNETCORE_ENVIRONMENT = new EnvironmentSetting("ASPNETCORE_ENVIRONMENT");
		public readonly static EnvironmentSetting DOTNET_ENVIRONMENT = new EnvironmentSetting("DOTNET_ENVIRONMENT");
		public bool IsProd => string.Equals(Value, "production", StringComparison.InvariantCultureIgnoreCase);
	}
}
