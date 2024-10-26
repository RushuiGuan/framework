using Sample.Core.Commands.MyOwnNameSpace;

namespace Sample.Core.Commands {
	public record class EfficiencyTestComand : ISystemCommand {
		public bool Callback { get; set; }

		public int Id { get; set; }
		/// <summary>
		/// Duration in ms
		/// </summary>
		public int Duration { get; set; }

		/// <summary>
		/// If true, perform CPU intensive operations.  Otherwise run Task.Sleep
		/// </summary>
		public bool CPUBound { get; set; }

		public EfficiencyTestComand? SubCommand { get; set; }
		public int SubCommandCount { get; set; }
	}
}