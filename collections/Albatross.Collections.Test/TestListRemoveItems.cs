using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace Albatross.Collections.Test {
	public class TestListRemoveItems {
		const int seed = 9999;
		const int min = 1;
		const int max = 1000;
		Random random = new Random(seed);
		const int SampleSize_Tiny = 100;
		const int SampleSize_Tiny_2 = 1000;
		const int SampleSize_Small = 10000;
		const int SampleSize_Medium = 1000000;
		const int SampleSize_Large = 100000000;
		Predicate<int> predicate = (int i) => i >= 500;

		List<int> BuildList(int count) {
			var list = new List<int>();
			for (int i = 0; i < count; i++) {
				list.Add(random.Next(min, max));
			}
			return list;
		}

		[Theory]
		[InlineData(SampleSize_Tiny)]
		[InlineData(SampleSize_Tiny_2)]
		[InlineData(SampleSize_Small)]
		[InlineData(SampleSize_Medium)]
		// [InlineData(SampleSize_Large)] // this is too slow
		public void RemoveFromTheBack(int count) {
			var list = BuildList(count);

			Stopwatch stopwatch = Stopwatch.StartNew();
			for (int i = list.Count - 1; i >= 0; i--) {
				if (predicate(list[i])) {
					list.RemoveAt(i);
				}
			}
			stopwatch.Stop();
			Console.WriteLine($"RemoveFromTheBack: {count} items, {stopwatch.ElapsedMilliseconds:#,#0} ms,  {stopwatch.ElapsedTicks:#,#0}  ticks");
		}

		[Theory]
		[InlineData(SampleSize_Tiny)]
		[InlineData(SampleSize_Tiny_2)]
		[InlineData(SampleSize_Small)]
		[InlineData(SampleSize_Medium)]
		// [InlineData(SampleSize_Large)] // this is too slow
		public void RemoveFromTheFront(int count) {
			var list = BuildList(count);

			Stopwatch stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < list.Count; i++) {
				if (predicate(list[i])) {
					list.RemoveAt(i);
				}
			}
			stopwatch.Stop();
			Console.WriteLine($"RemoveFromTheFront: {count} items, {stopwatch.ElapsedMilliseconds:#,#0} ms,  {stopwatch.ElapsedTicks:#,#0}  ticks");
		}

		[Theory]
		[InlineData(SampleSize_Tiny)]
		[InlineData(SampleSize_Tiny_2)]
		[InlineData(SampleSize_Small)]
		[InlineData(SampleSize_Medium)]
		[InlineData(SampleSize_Large)]
		public void LinearOperation(int count) {
			var list = BuildList(count);

			Stopwatch stopwatch = Stopwatch.StartNew();
			var newList = new List<int>();
			foreach(var item in list) {
				if (!predicate(item)) {
					newList.Add(item);
				}
			}
			list.Clear();
			list.AddRange(newList);
			stopwatch.Stop();
			Console.WriteLine($"LinearOperation: {count} items, {stopwatch.ElapsedMilliseconds:#,#0} ms, {stopwatch.ElapsedTicks:#,#0} ticks");
		}
		[Theory]
		[InlineData(SampleSize_Tiny)]
		[InlineData(SampleSize_Tiny_2)]
		[InlineData(SampleSize_Small)]
		[InlineData(SampleSize_Medium)]
		[InlineData(SampleSize_Large)]
		public void LinearOperationWithoutCopyBack(int count) {
			var list = BuildList(count);

			Stopwatch stopwatch = Stopwatch.StartNew();
			var newList = new List<int>();
			foreach (var item in list) {
				if (!predicate(item)) {
					newList.Add(item);
				}
			}
			stopwatch.Stop();
			Console.WriteLine($"LinearOperationWithoutCopyBack: {count} items, {stopwatch.ElapsedMilliseconds:#,#0} ms, {stopwatch.ElapsedTicks:#,#0} ticks");
		}

		[Theory]
		[InlineData(SampleSize_Tiny)]
		[InlineData(SampleSize_Tiny_2)]
		[InlineData(SampleSize_Small)]
		[InlineData(SampleSize_Medium)]
		[InlineData(SampleSize_Large)]
		public void RemoveAnyMethod(int count) {
			var list = BuildList(count);

			Stopwatch stopwatch = Stopwatch.StartNew();
			var newList = new List<int>();
			list.RemoveAny(this.predicate);
			stopwatch.Stop();
			Console.WriteLine($"RemoveAnyMethod: {count} items, {stopwatch.ElapsedMilliseconds:#,#0} ms, {stopwatch.ElapsedTicks:#,#0} ticks");
		}
	}
}
