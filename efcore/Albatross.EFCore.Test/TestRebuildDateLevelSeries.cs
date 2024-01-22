using Albatross.EFCore.DateLevel;
using Albatross.Hosting.Test;
using System.Collections.Generic;
using System.Linq;
using Xunit;

#if NET8_0
namespace Albatross.EFCore.Test {
	public class TestRebuildDateLevelSeries {
		[Fact]
		public void NoOp() {
			List<TickSize2> list = new List<TickSize2>();
			var input = new TestAsyncEnumerableQuery<TickSize2>(list);
			var items = list.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args=>list.Remove(args));
		}
		[Fact]
		public void Single_Row() {
			List<TickSize2> list = new List<TickSize2> {
				new TickSize2(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan1_2022
				}
			};
			var input = new TestAsyncEnumerableQuery<TickSize2>(list);
			var items = list.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args=>list.Remove(args));
			Assert.Collection(input, args=>Assert.Equal(DateLevelEntity.MaxEndDate, args.EndDate));
		}
		[Fact]
		public void Two_Row_Diff() {
			List<TickSize2> list = new List<TickSize2> {
				new TickSize2(1, DateOnlyValues.Feb1_2022, 100){
					EndDate = DateOnlyValues.Jan1_2022
				},
				new TickSize2(1, DateOnlyValues.Jan1_2022, 200){
					EndDate = DateOnlyValues.Jan1_2022
				}
			};
			var input = new TestAsyncEnumerableQuery<TickSize2>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args=>list.Remove(args));
			Assert.Collection(input, 
				args => {
					Assert.Equal(DateOnlyValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jan31_2022, args.EndDate);
				}
			);
		}
		[Fact]
		public void Two_Row_Same() {
			List<TickSize2> list = new List<TickSize2> {
				new TickSize2(1, DateOnlyValues.Feb1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new TickSize2(1, DateOnlyValues.Jan1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<TickSize2>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args => list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(100, args.Value);
				}
			);
		}
		[Fact]
		public void Three_Row_Diff() {
			List<TickSize2> list = new List<TickSize2> {
				new TickSize2(1, DateOnlyValues.Feb1_2022, 100){ EndDate = DateOnlyValues.Jan1_2022 },
				new TickSize2(1, DateOnlyValues.Jan1_2022, 200) { EndDate = DateOnlyValues.Jan1_2022 },
				new TickSize2(1, DateOnlyValues.Mar1_2022, 300) { EndDate = DateOnlyValues.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<TickSize2>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args => list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(DateOnlyValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Feb28_2022, args.EndDate);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jan31_2022, args.EndDate);
				},
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
				}
			);
		}
		[Fact]
		public void Three_Row_Same() {
			List<TickSize2> list = new List<TickSize2> {
				new TickSize2(1, DateOnlyValues.Feb1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new TickSize2(1, DateOnlyValues.Jan1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new TickSize2(1, DateOnlyValues.Mar1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<TickSize2>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args => list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(100, args.Value);
				}
			);
		}
		[Fact]
		public void Three_Row_Mixed() {
			List<TickSize2> list = new List<TickSize2> {
				new TickSize2(1, DateOnlyValues.Feb1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new TickSize2(1, DateOnlyValues.Jan1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new TickSize2(1, DateOnlyValues.Mar1_2022, 200) { EndDate = DateOnlyValues.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<TickSize2>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args => list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Feb28_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}
	}
}
#endif