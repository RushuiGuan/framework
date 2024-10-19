using Sample.Core.Commands.MyOwnNameSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Commands {
	public record class EfficiencyTestComand : ISystemCommand {
		public int Id { get; set; }
		/// <summary>
		/// Duration in ms
		/// </summary>
		public int Duration { get; set; }

		/// <summary>
		/// If true, perform CPU intensive operations.  Otherwise run Task.Sleep
		/// </summary>
		public bool CPUIntensive { get; set; }

		public EfficiencyTestComand SubCommand { get; set; } = new EfficiencyTestComand();
		public int SubCommandCount { get; set; }
	}
}
