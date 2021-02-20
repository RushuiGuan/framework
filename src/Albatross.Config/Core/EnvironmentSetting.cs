using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.Core {
	/// <summary>
	/// This is created because IHostEnvironment will return production if ASPNETCORE_ENVIRONMENT OR DOTNET_ENVIRONMENT
	/// variable is not set
	/// </summary>
	public class EnvironmentSetting {
		public string Value { get; }

		public EnvironmentSetting(string variable) {
			Value = System.Environment.GetEnvironmentVariable(variable)?.ToLower();
		}

		public readonly static EnvironmentSetting ASPNETCORE_ENVIRONMENT = new EnvironmentSetting("ASPNETCORE_ENVIRONMENT");
		public readonly static EnvironmentSetting DOTNET_ENVIRONMENT = new EnvironmentSetting("DOTNET_ENVIRONMENT");
	}
}
