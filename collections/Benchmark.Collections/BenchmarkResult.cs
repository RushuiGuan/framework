namespace Benchmark.Collections {
	public record class BenchmarkResult {
		public BenchmarkResult(string testName, string parameter) {
			this.TestName = testName;
			this.Parameter = parameter;
		}
		public string TestName { get; set; }
		public string Parameter { get; set; }
		public long ElapsedMilliseconds { get; set; }
		public long ElapsedTicks { get; set; }
	}

	public record class Report {
		public Report(string testName, string parameter) {
			this.TestName = testName;
			this.Parameter = parameter;
		}
		public string TestName { get; set; }
		public string Parameter { get; set; }
		public int Count { get; set; }
		public long AverageDuration { get; set; }
		public long AverageTicks { get; set; }
	}
}