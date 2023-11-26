using Albatross.EFCore.DateLevel;
using Albatross.Hosting.Test;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Albatross.EFCore.Test {
	public class TestRebuildDateLevelSeries {
		[Fact]
		public void NoOp() {
			List<TickSize> list = new List<TickSize>();
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			var items = list.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args=>list.Remove(args));
		}
		[Fact]
		public void Single_Row() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Jan1_2022, 100) {
					EndDate = Values.Jan1_2022
				}
			};
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			var items = list.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args=>list.Remove(args));
			Assert.Collection(input, args=>Assert.Equal(DateLevelEntity.MaxEndDate, args.EndDate));
		}
		[Fact]
		public void Two_Row_Diff() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Feb1_2022, 100){
					EndDate = Values.Jan1_2022
				},
				new TickSize(1, Values.Jan1_2022, 200){
					EndDate = Values.Jan1_2022
				}
			};
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args=>list.Remove(args));
			Assert.Collection(input, 
				args => {
					Assert.Equal(Values.Feb1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
				},
				args => {
					Assert.Equal(Values.Jan1_2022, args.StartDate);
					Assert.Equal(Values.Jan31_2022, args.EndDate);
				}
			);
		}
		[Fact]
		public void Two_Row_Same() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Feb1_2022, 100) { EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Jan1_2022, 100) { EndDate = Values.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args => list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(Values.Jan1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(100, args.Value);
				}
			);
		}
		[Fact]
		public void Three_Row_Diff() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Feb1_2022, 100){ EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Jan1_2022, 200) { EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Mar1_2022, 300) { EndDate = Values.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args => list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(Values.Feb1_2022, args.StartDate);
					Assert.Equal(Values.Feb28_2022, args.EndDate);
				},
				args => {
					Assert.Equal(Values.Jan1_2022, args.StartDate);
					Assert.Equal(Values.Jan31_2022, args.EndDate);
				},
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
				}
			);
		}
		[Fact]
		public void Three_Row_Same() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Feb1_2022, 100) { EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Jan1_2022, 100) { EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Mar1_2022, 100) { EndDate = Values.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args => list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(Values.Jan1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(100, args.Value);
				}
			);
		}
		[Fact]
		public void Three_Row_Mixed() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Feb1_2022, 100) { EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Jan1_2022, 100) { EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Mar1_2022, 200) { EndDate = Values.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args => list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(Values.Jan1_2022, args.StartDate);
					Assert.Equal(Values.Feb28_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}
	}
}
