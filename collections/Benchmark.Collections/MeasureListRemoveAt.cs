using System;
using Albatross.Collections;
using Albatross.CommandLine;
using Albatross.Text;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;

namespace Benchmark.Collections {
	[Verb("measure-listitem-removal", typeof(MeasureListItemRemoval), description: "Measure the performance of removing items from a list using RemoveAt")]
	public class MeasureListItemRemovalOptions {
		[Option(Alias = ["c"])]
		public int Count { get; set; }

		[Option(Alias = ["cutoff"])]
		public int? AlgoCutoff { get; set; }

		[Option(Alias = ["r"])]
		public int? Repeat { get; set; }
	}

	public class MeasureListItemRemoval : ICommandHandler {
		const int seed = 9999;
		const int min = 1;
		const int max = 1000;
		const int AlgoCutoff = 100;
		static readonly Random random = new Random(seed);
		static Predicate<int> predicate = (int i) => i >= 500;
		private MeasureListItemRemovalOptions options;

		public MeasureListItemRemoval(IOptions<MeasureListItemRemovalOptions> options) {
			this.options = options.Value;
		}

		static List<int> BuildList(int count) {
			var list = new List<int>();
			for (int i = 0; i < count; i++) {
				list.Add(random.Next(min, max));
			}
			return list;
		}

		public static void RemoveFromTheRear(List<BenchmarkResult> results, int count) {
			var list = BuildList(count);
			Stopwatch stopwatch = Stopwatch.StartNew();
			list.RemoveAny_FromRear(predicate);
			stopwatch.Stop();
			results.Add(new BenchmarkResult(MethodBase.GetCurrentMethod()?.Name ?? string.Empty) {
				Count = count,
				ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
				ElapsedTicks = stopwatch.ElapsedTicks
			});
		}

		public static void RemoveFromTheFront(List<BenchmarkResult> results, int count) {
			var list = BuildList(count);
			Stopwatch stopwatch = Stopwatch.StartNew();
#pragma warning disable CS0618 // Type or member is obsolete
			list.RemoveAny_FromFront(predicate);
#pragma warning restore CS0618 // Type or member is obsolete
			stopwatch.Stop();
			results.Add(new BenchmarkResult(MethodBase.GetCurrentMethod()?.Name ?? string.Empty) {
				Count = count,
				ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
				ElapsedTicks = stopwatch.ElapsedTicks
			});
		}

		public static void RemoveWithNewList(List<BenchmarkResult> results, int count) {
			var list = BuildList(count);
			Stopwatch stopwatch = Stopwatch.StartNew();
			list.RemoveAny_WithNewList(predicate);
			stopwatch.Stop();
			results.Add(new BenchmarkResult(MethodBase.GetCurrentMethod()?.Name ?? string.Empty) {
				Count = count,
				ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
				ElapsedTicks = stopwatch.ElapsedTicks
			});
		}

		public static void RemoveAnyWithAlgoSelection(List<BenchmarkResult> results, int count, int algoCutoff) {
			var list = BuildList(count);
			Stopwatch stopwatch = Stopwatch.StartNew();
			list.RemoveAny(predicate, algoCutoff);
			stopwatch.Stop();
			var name = $"{nameof(RemoveAnyWithAlgoSelection)}-{algoCutoff}";
			results.Add(new BenchmarkResult(name) {
				Count = count,
				ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
				ElapsedTicks = stopwatch.ElapsedTicks
			});
		}

		public int Invoke(InvocationContext context) => throw new NotImplementedException();


		void WarmUp() {
		}

		void RunTest(List<BenchmarkResult> results, int seed) {
			var random = new Random(seed);
			RemoveFromTheFront(results, this.options.Count);
			RemoveFromTheRear(results, this.options.Count);
			RemoveWithNewList(results, this.options.Count);
			RemoveAnyWithAlgoSelection(results, this.options.Count, this.options.AlgoCutoff ?? 100);
		}

		public async Task<int> InvokeAsync(InvocationContext context) {
			//first call the methods to warm up the JIT
			var list = new List<BenchmarkResult>();
			int warmUpIndex;
			for (warmUpIndex = 0; warmUpIndex < 10; warmUpIndex++) {
				RunTest(list, warmUpIndex);
			}

			// run the test now
			list = new List<BenchmarkResult>();
			for (int i = 0; i < (this.options.Repeat ?? 100); i++) {
				RunTest(list, i + warmUpIndex);
			}

			var report = list.GroupBy(x => x.Test).Select(x => new BenchmarkResult(x.Key) {
				Count = x.First().Count,
				ElapsedMilliseconds = Convert.ToInt64(x.Average(args => args.ElapsedMilliseconds)),
				ElapsedTicks = Convert.ToInt64(x.Average(args => args.ElapsedTicks))
			}).ToArray();


			var options = new PrintOptionBuilder<PrintTableOption>()
				.Property("Test", "Count", "ElapsedMilliseconds", "ElapsedTicks")
				.Format("Count", "#,#0")
				.Format("ElapsedMilliseconds", "#,#0")
				.Format("ElapsedTicks", "#,#0")
				.Build();
			await Console.Out.PrintTable(report, options);
			return 0;
		}
	}
}
