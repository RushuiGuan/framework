using Albatross.Hosting.Test;
using Albatross.Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Repository.Test {
	public class TestRebuildDateLevelSeries {
		[Fact]
		public async Task NoOp() {
			List<TickSize> list = new List<TickSize>();
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			await input.AsQueryable().RebuildDateLevelSeries(1, args=>list.Remove(args));
		}
		[Fact]
		public async Task Single_Row() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Jan1_2022, 100) {
					EndDate = Values.Jan1_2022
				}
			};
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			await input.AsQueryable().RebuildDateLevelSeries(1, args=>list.Remove(args));
			Assert.Collection(input, args=>Assert.Equal(DateLevelEntity.MaxEndDate, args.EndDate));
		}
		[Fact]
		public async Task Two_Row_Diff() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Feb1_2022, 100){
					EndDate = Values.Jan1_2022
				},
				new TickSize(1, Values.Jan1_2022, 200){
					EndDate = Values.Jan1_2022
				}
			};
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			await input.AsQueryable().RebuildDateLevelSeries(1, args=>list.Remove(args));
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
		public async Task Two_Row_Same() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Feb1_2022, 100) { EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Jan1_2022, 100) { EndDate = Values.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			await input.AsQueryable().RebuildDateLevelSeries(1, args=>list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(Values.Jan1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(100, args.Value);
				}
			);
		}
		[Fact]
		public async Task Three_Row_Diff() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Feb1_2022, 100){ EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Jan1_2022, 200) { EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Mar1_2022, 300) { EndDate = Values.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			await input.AsQueryable().RebuildDateLevelSeries(1, args=>list.Remove(args));
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
		public async Task Three_Row_Same() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Feb1_2022, 100) { EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Jan1_2022, 100) { EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Mar1_2022, 100) { EndDate = Values.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			await input.AsQueryable().RebuildDateLevelSeries(1, args => list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(Values.Jan1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(100, args.Value);
				}
			);
		}
		[Fact]
		public async Task Three_Row_Mixed() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Feb1_2022, 100) { EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Jan1_2022, 100) { EndDate = Values.Jan1_2022 },
				new TickSize(1, Values.Mar1_2022, 200) { EndDate = Values.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<TickSize>(list);
			await input.AsQueryable().RebuildDateLevelSeries(1, args => list.Remove(args));
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
