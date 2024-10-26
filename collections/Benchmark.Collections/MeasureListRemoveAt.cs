using Albatross.Collections;
using Albatross.CommandLine;
using Albatross.Text;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Benchmark.Collections {
	[Verb("measure-listitem-removal", typeof(MeasureListItemRemoval), Description = "Measure the performance of removing items from a list using RemoveAt")]
	public class MeasureListItemRemovalOptions {
		public int ListSize { get; set; }

		[Option("cutoff", Description = "Used to determine the list size cut off point to switch from RemoveAny_FromRear to RemoveAny_WithNewList.  Default is 100")]
		public int? AlgoCutoff { get; set; }

		[Option("c", Description = "How many times the test will be run, default is 100")]
		public int? Count { get; set; }
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

		public static void RemoveFromTheRear(List<BenchmarkResult> results, int listSize) {
			var list = BuildList(listSize);
			Stopwatch stopwatch = Stopwatch.StartNew();
			list.RemoveAny_FromRear(predicate);
			stopwatch.Stop();
			results.Add(new BenchmarkResult(nameof(RemoveFromTheRear), $"listSize={listSize:#,#}") {
				ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
				ElapsedTicks = stopwatch.ElapsedTicks
			});
		}

		public static void RemoveWithNewList(List<BenchmarkResult> results, int listSize) {
			var list = BuildList(listSize);
			Stopwatch stopwatch = Stopwatch.StartNew();
			list.RemoveAny_WithNewList(predicate);
			stopwatch.Stop();
			results.Add(new BenchmarkResult(nameof(RemoveWithNewList), $"listSize={listSize:#,#}") {
				ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
				ElapsedTicks = stopwatch.ElapsedTicks
			});
		}

		public static void RemoveAnyWithAlgoSelection(List<BenchmarkResult> results, int listSize, int algoCutoff) {
			var list = BuildList(listSize);
			Stopwatch stopwatch = Stopwatch.StartNew();
			list.RemoveAny(predicate, algoCutoff);
			stopwatch.Stop();
			results.Add(new BenchmarkResult(nameof(RemoveAnyWithAlgoSelection), $"listSize={listSize:#,#}, algoCutOff={algoCutoff:#,#}") {
				ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
				ElapsedTicks = stopwatch.ElapsedTicks
			});
		}

		public int Invoke(InvocationContext context) => throw new NotImplementedException();


		void RunTest(List<BenchmarkResult> results, int seed) {
			var random = new Random(seed);
			RemoveFromTheRear(results, this.options.ListSize);
			RemoveWithNewList(results, this.options.ListSize);
			RemoveAnyWithAlgoSelection(results, this.options.ListSize, this.options.AlgoCutoff ?? 100);
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
			for (int i = 0; i < (this.options.Count ?? 100); i++) {
				RunTest(list, i + warmUpIndex);
			}

			var options = new PrintOptionBuilder<PrintTableOption>()
				.Property("TestName", "Parameter", "Count", "AverageDuration", "AverageTicks")
				.Format("Count", "#,#0")
				.Format("AverageDuration", "#,#0")
				.Format("AverageTicks", "#,#0")
				.Build();
			await Console.Out.PrintTable(list.BuildReports().ToArray(), options);
			return 0;
		}
	}
}