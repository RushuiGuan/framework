namespace Benchmark.Collections {

	public record class BenchmarkResult {
		public BenchmarkResult(string test) {
			this.Test = test;
		}
		public string Test { get; set; }
		public int Count { get; set; }
		public long ElapsedMilliseconds { get; set; }
		public long ElapsedTicks { get; set; }
	}
}
