using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchmark.Collections {
	public static class Extensions {
		public static IEnumerable<Report> BuildReports(IEnumerable<BenchmarkResult> results) {
			return results.GroupBy(x => x.Test)
				.Select(x => new Report(x.Key) {
					Count = x.Count(),
					AverageDuration = (long)x.Average(args => args.ElapsedMilliseconds),
					AverageTicks = (long)x.Average(args => args.ElapsedTicks)
				});
		}
	}
}
