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

namespace Benchmark.Collections {
	[Verb("measure-listitem-removal", typeof(MeasureListItemRemoval), description: "Measure the performance of removing items from a list using RemoveAt")]
	public class MeasureListItemRemovalOptions {
		[Option(Alias = ["c"])]
		public int Count { get; set; }
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

		public static void RemoveFromTheBack(List<BenchmarkResult> results, int count) {
			var list = BuildList(count);

			Stopwatch stopwatch = Stopwatch.StartNew();
			list.RemoveAny_FromBack(predicate);
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

		public static void LinearOperation(List<BenchmarkResult> results, int count) {
			var list = BuildList(count);

			Stopwatch stopwatch = Stopwatch.StartNew();
			list.RemoveAny_Linear(predicate);
			stopwatch.Stop();
			results.Add(new BenchmarkResult(MethodBase.GetCurrentMethod()?.Name ?? string.Empty) {
				Count = count,
				ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
				ElapsedTicks = stopwatch.ElapsedTicks
			});
		}

		public static void RemoveAnyMethod(List<BenchmarkResult> results, int count) {
			var list = BuildList(count);

			Stopwatch stopwatch = Stopwatch.StartNew();
			var newList = new List<int>();
			list.RemoveAny(predicate);
			stopwatch.Stop();
			results.Add(new BenchmarkResult(MethodBase.GetCurrentMethod()?.Name ?? string.Empty) {
				Count = count,
				ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
				ElapsedTicks = stopwatch.ElapsedTicks
			});
		}

		public int Invoke(InvocationContext context) => throw new NotImplementedException();


		public async Task<int> InvokeAsync(InvocationContext context) {
			//first call the methods to warm up the JIT
			var list = new List<BenchmarkResult>();
			RemoveFromTheFront(list, 0);
			RemoveFromTheBack(list, 0);
			LinearOperation(list, 0);
			RemoveAnyMethod(list, 0);

			// run the test now
			list = new List<BenchmarkResult>();
			RemoveFromTheFront(list, this.options.Count);
			RemoveFromTheBack(list, this.options.Count);
			LinearOperation(list, this.options.Count);
			RemoveAnyMethod(list, this.options.Count);

			var options = new PrintOptionBuilder<PrintTableOption>()
				.Property("Test", "Count", "ElapsedMilliseconds", "ElapsedTicks")
				.Format("Count", "#,#0")
				.Format("ElapsedMilliseconds", "#,#0")
				.Format("ElapsedTicks", "#,#0")
				.Build();
			await Console.Out.PrintTable(list.ToArray(), options);
			return 0;
		}
	}
}
