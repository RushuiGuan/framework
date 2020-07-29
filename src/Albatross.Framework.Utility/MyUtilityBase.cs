using Albatross.Hosting.Utility;
using Albatross.Logging;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Framework.Utility {
	public abstract class MyUtilityBase<T> : UtilityBase<T> {
		protected MyUtilityBase(T option) : base(option) {
		}

		protected override void ConfigureLogging(LoggerConfiguration cfg) {
			SetupSerilog.UseConfigFile(cfg, "serilog.json", null);
		}
	}
}
