namespace Benchmark.Collections {

	public record class BenchmarkResult {
		public BenchmarkResult(string test) {
			this.Test = test;
		}
		public string Test { get; set; }
		public long ElapsedMilliseconds { get; set; }
		public long ElapsedTicks { get; set; }
	}
	public record class Report {
		public Report(string name) {
			this.TestName = name;
		}
		public string TestName { get; set; }
		public int Count { get; set; }
		public long AverageDuration { get; set; }
		public long AverageTicks { get; set; }
	}
}
