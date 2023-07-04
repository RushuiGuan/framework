using Albatross.Hosting.Utility;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	/// <summary>
	/// for collections with about 100 items, there is no difference in performance between dictionary and array.
	/// however, dictionary offers consistent seek performance for larger collections, where array performance suffers linearly.
	/// </summary>
	[Verb("dict-vs-list-seek")]
	public class DictionaryVsListSeekPerformanceOption : BaseOption { }

	public class DictionaryVsArraySeekPerformance : UtilityBase<DictionaryVsListSeekPerformanceOption> {
		public DictionaryVsArraySeekPerformance(DictionaryVsListSeekPerformanceOption option) : base(option) { }

		public Task<int> RunUtility() {
			RunTest(100);

			RunTest(10);
			RunTest(100);
			RunTest(200);
			RunTest(300);
			RunTest(400);
			RunTest(500);
			RunTest(600);
			RunTest(1000);
			RunTest(10000);
			RunTest(100000);
			RunTest(1000000);
			RunTest(10000000);
			return Task.FromResult(0);
		}

		void RunTest(int count) {
			Dictionary<int, int> dict = new Dictionary<int, int>();
			int[] list = new int[count];

			for (int i = 0; i < count; i++) {
				dict[i] = i;
				list[i] = i;
			}

			var target = count / 2;

			Stopwatch stopwatch = Stopwatch.StartNew();
			var a = dict[target];
			var dictPerf = stopwatch.ElapsedTicks;

			stopwatch.Restart();
			int b = 0;
			foreach (var value in list) {
				if (value == target) {
					b = value; break;
				}
			}
			var listPerf = stopwatch.ElapsedTicks;
			if(a != b) {
				throw new Exception("something wrong with your logic");
			}
			Options.WriteOutput($"for {count} items: dictionary = {dictPerf:#,#0}, list = {listPerf:#,#0}");
		}
	}
}
