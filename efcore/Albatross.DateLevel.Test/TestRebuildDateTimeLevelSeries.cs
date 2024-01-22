using Albatross.Hosting.Test;
using Sample.EFCore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Albatross.DateLevel.Test {
	public class TestRebuildDateTimeLevelSeries {
		[Fact]
		public void NoOp() {
			List<ContractSpec> list = new List<ContractSpec>();
			var input = new TestAsyncEnumerableQuery<ContractSpec>(list);
			var items = list.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args=>list.Remove(args));
		}
		[Fact]
		public void Single_Row() {
			List<ContractSpec> list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Jan1_2022, 100) {
					EndDate = DateTimeValues.Jan1_2022
				}
			};
			var input = new TestAsyncEnumerableQuery<ContractSpec>(list);
			var items = list.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args=>list.Remove(args));
			Assert.Collection(input, args=>Assert.Equal(DateTimeLevelEntity.MaxEndDate, args.EndDate));
		}
		[Fact]
		public void Two_Row_Diff() {
			List<ContractSpec> list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Feb1_2022, 100){
					EndDate = DateTimeValues.Jan1_2022
				},
				new ContractSpec(1, DateTimeValues.Jan1_2022, 200){
					EndDate = DateTimeValues.Jan1_2022
				}
			};
			var input = new TestAsyncEnumerableQuery<ContractSpec>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args=>list.Remove(args));
			Assert.Collection(input, 
				args => {
					Assert.Equal(DateTimeValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
				},
				args => {
					Assert.Equal(DateTimeValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jan31_2022, args.EndDate);
				}
			);
		}
		[Fact]
		public void Two_Row_Same() {
			List<ContractSpec> list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Feb1_2022, 100) { EndDate = DateTimeValues.Jan1_2022 },
				new ContractSpec(1, DateTimeValues.Jan1_2022, 100) { EndDate = DateTimeValues.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<ContractSpec>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args => list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(DateTimeValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(100, args.Value);
				}
			);
		}
		[Fact]
		public void Three_Row_Diff() {
			List<ContractSpec> list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Feb1_2022, 100){ EndDate = DateTimeValues.Jan1_2022 },
				new ContractSpec(1, DateTimeValues.Jan1_2022, 200) { EndDate = DateTimeValues.Jan1_2022 },
				new ContractSpec(1, DateTimeValues.Mar1_2022, 300) { EndDate = DateTimeValues.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<ContractSpec>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args => list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(DateTimeValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Feb28_2022, args.EndDate);
				},
				args => {
					Assert.Equal(DateTimeValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jan31_2022, args.EndDate);
				},
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
				}
			);
		}
		[Fact]
		public void Three_Row_Same() {
			List<ContractSpec> list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Feb1_2022, 100) { EndDate = DateTimeValues.Jan1_2022 },
				new ContractSpec(1, DateTimeValues.Jan1_2022, 100) { EndDate = DateTimeValues.Jan1_2022 },
				new ContractSpec(1, DateTimeValues.Mar1_2022, 100) { EndDate = DateTimeValues.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<ContractSpec>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args => list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(DateTimeValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(100, args.Value);
				}
			);
		}
		[Fact]
		public void Three_Row_Mixed() {
			List<ContractSpec> list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Feb1_2022, 100) { EndDate = DateTimeValues.Jan1_2022 },
				new ContractSpec(1, DateTimeValues.Jan1_2022, 100) { EndDate = DateTimeValues.Jan1_2022 },
				new ContractSpec(1, DateTimeValues.Mar1_2022, 200) { EndDate = DateTimeValues.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<ContractSpec>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries(args => list.Remove(args));
			Assert.Collection(input,
				args => {
					Assert.Equal(DateTimeValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Feb28_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}
	}
}
