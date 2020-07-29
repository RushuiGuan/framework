using Albatross.Hosting.Utility;
using Albatross.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Framework.Utility {
	public abstract class MyUtilityBase<T> : UtilityBase<T> {
		protected MyUtilityBase(T option) : base(option) {
		}

		protected override Logger SetupLogging(T option) {
			return new SetupSerilog().UseConfigFile("serilog.json").Create();
		}
	}
}
