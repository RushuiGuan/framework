using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.Config.Core {
	public class ProgramSetting {
		public const string Key = "program";

		static readonly string[] ProductionEnvirionment = new string[] {
			"prod", "production",
		};

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
