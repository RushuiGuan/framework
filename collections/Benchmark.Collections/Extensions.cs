using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchmark.Collections {
	public static class Extensions {
		public static IEnumerable<Report> BuildReports(this IEnumerable<BenchmarkResult> results) {
			return results.GroupBy(x => new { x.TestName, x.Parameter })
				.Select(x => new Report(x.Key.TestName, x.Key.Parameter) {
					Count = x.Count(),
					AverageDuration = (long)x.Average(args => args.ElapsedMilliseconds),
					AverageTicks = (long)x.Average(args => args.ElapsedTicks)
				});
		}
	}
}