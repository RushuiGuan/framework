using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace Albatross.Config {
	public class EnvironmentSetting {
		public EnvironmentSetting(IHostEnvironment hostEnvironment) {
			this.Value = hostEnvironment.EnvironmentName;
		}
		public string Value { get;  }

		static readonly string[] ProductionEnvirionment = new string[] {
			"prod", "production",
		};
		public bool IsProd => ProductionEnvirionment.Contains(Value?.ToLower());
	}
}
