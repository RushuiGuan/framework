using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.Config.Core {
	public class ProgramSetting {
		public const string Key = "program";
		public const string WindowsServiceManager = "windows";
		public const string SystemDServiceManager = "systemd";

		static readonly string[] ProductionEnvirionment = new string[] {
			"prod", "production",
		};

		/// <summary>
		/// systemd or windows, used by the Worker host
		/// </summary>
		public string ServiceManager { get; set; }
		public string Environment { get; set; }
		public bool IsProd => ProductionEnvirionment.Contains(Environment?.ToLower());
        /// <summary>
        /// Required: name of the application
        /// </summary>
		public string App { get; set; }
        /// <summary>
        /// Optional: the group of the application
        /// </summary>
        public string Group { get; set; }
	}
}
