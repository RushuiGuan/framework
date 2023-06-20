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
	[Verb("dict-vs-list-iterate")]
	public class DictionaryVsListIteratePerformanceOption : BaseOption { }

	public class DictionaryVsArrayIteratePerformance : UtilityBase<DictionaryVsListIteratePerformanceOption> {
		public DictionaryVsArrayIteratePerformance(DictionaryVsListIteratePerformanceOption option) : base(option) { }

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
			List<int> list = new List<int>();
			HashSet<int> set = new HashSet<int>();
			int[] array = new int[count];

			for (int i = 0; i < count; i++) {
				dict[i] = i;
				list.Add(i);
				set.Add(i);
				array[i] = i;
			}

			Stopwatch stopwatch = Stopwatch.StartNew();
			foreach(var item in dict.Values) {
				item.ToString();
			}
			var dictPerf = stopwatch.ElapsedTicks;

			stopwatch.Restart();
			foreach (var value in array) {
				value.ToString();
			}
			var arrayPerf = stopwatch.ElapsedTicks;

			stopwatch.Restart();
			foreach(var value in list) {
				value.ToString();
			}
			var listPerf = stopwatch.ElapsedTicks;

			stopwatch.Restart();
			foreach(var value in  set) {
				value.ToString();
			}
			var setPerf = stopwatch.ElapsedTicks;
			Options.WriteOutput($"for {count} items: dictionary = {dictPerf:#,#0}, array = {arrayPerf:#,#0}, list = {listPerf:#,#0}, set = {setPerf:#,#0}");
		}
	}
}
