using Albatross.Testing.EFCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace Albatross.DateLevel.Test {
	public class TestRebuildDateLevelSeries {
		[Fact]
		public void NoOp() {
			List<SpreadSpec> list = new List<SpreadSpec>();
			list.RebuildDateLevelSeries<SpreadSpec, int>(args => list.Remove(args));
		}
		[Fact]
		public void Single_Row() {
			List<SpreadSpec> list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan1_2022
				}
			};
			list.RebuildDateLevelSeries<SpreadSpec, int>(args => list.Remove(args));
			Assert.Collection(list, args => {
				Assert.Equal(1, args.Key);
				Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
				Assert.Equal(IDateLevelEntity.MaxEndDate, args.EndDate);
			});
		}
		[Fact]
		public void Single_Row_with_multiple_keys() {
			List<SpreadSpec> list = new List<SpreadSpec> {
				new SpreadSpec(0, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan1_2022
				},
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan1_2022
				},
				new SpreadSpec(2, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan1_2022
				}
			};
			list.RebuildDateLevelSeries<SpreadSpec, int>(args => list.Remove(args));
			Assert.Collection(list, args => {
				Assert.Equal(0, args.Key);
				Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
				Assert.Equal(IDateLevelEntity.MaxEndDate, args.EndDate);
			},
			args => {
				Assert.Equal(1, args.Key);
				Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
				Assert.Equal(IDateLevelEntity.MaxEndDate, args.EndDate);
			},
			args => {
				Assert.Equal(2, args.Key);
				Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
				Assert.Equal(IDateLevelEntity.MaxEndDate, args.EndDate);
			});
		}
		[Fact]
		public void Two_Row_Diff() {
			List<SpreadSpec> list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Feb1_2022, 100){
					EndDate = DateOnlyValues.Jan1_2022
				},
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 200){
					EndDate = DateOnlyValues.Jan1_2022
				}
			};
			list.RebuildDateLevelSeries<SpreadSpec, int>(args => list.Remove(args));
			Assert.Collection(list,
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
			List<SpreadSpec> list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Feb1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
			};
			list.RebuildDateLevelSeries<SpreadSpec, int>(args => list.Remove(args));
			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(100, args.Value);
				}
			);
		}
		[Fact]
		public void Three_Row_Diff() {
			List<SpreadSpec> list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Feb1_2022, 100){ EndDate = DateOnlyValues.Jan1_2022 },
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 200) { EndDate = DateOnlyValues.Jan1_2022 },
				new SpreadSpec(1, DateOnlyValues.Mar1_2022, 300) { EndDate = DateOnlyValues.Jan1_2022 },
			};
			var input = new TestAsyncEnumerableQuery<SpreadSpec>(list);
			var items = input.Where(args => args.Key == 1);
			items.RebuildDateLevelSeries<SpreadSpec, int>(args => list.Remove(args));
			Assert.Collection((IEnumerable<SpreadSpec>)input,
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
			List<SpreadSpec> list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Feb1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
			};
			list.RebuildDateLevelSeries<SpreadSpec, int>(args => list.Remove(args));
			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(100, args.Value);
				}
			);
		}
		[Fact]
		public void Three_Row_Mixed() {
			List<SpreadSpec> list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Feb1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new SpreadSpec(1, DateOnlyValues.Mar1_2022, 200) { EndDate = DateOnlyValues.Jan1_2022 },
			};
			list.RebuildDateLevelSeries<SpreadSpec, int>(args => list.Remove(args));
			Assert.Collection(list,
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

		[Fact]
		public void Three_Row_Mixed_With_Multiple_Keys() {
			List<SpreadSpec> list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Feb1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new SpreadSpec(1, DateOnlyValues.Mar1_2022, 200) { EndDate = DateOnlyValues.Jan1_2022 },

				new SpreadSpec(2, DateOnlyValues.Jan1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new SpreadSpec(2, DateOnlyValues.Feb1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },
				new SpreadSpec(2, DateOnlyValues.Mar1_2022, 100) { EndDate = DateOnlyValues.Jan1_2022 },

				new SpreadSpec(3, DateOnlyValues.Jan1_2022, 200) { EndDate = DateOnlyValues.Jan1_2022 },
				new SpreadSpec(3, DateOnlyValues.Feb1_2022, 200) { EndDate = DateOnlyValues.Jan1_2022 },
				new SpreadSpec(3, DateOnlyValues.Mar1_2022, 200) { EndDate = DateOnlyValues.Jan1_2022 },
			};
			list.RebuildDateLevelSeries<SpreadSpec, int>(args => list.Remove(args));
			list = list.OrderBy(x => x.Key).ThenBy(x => x.StartDate).ToList();
			Assert.Collection(list,
				args => {
					Assert.Equal(1, args.Key);
					Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Feb28_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(1, args.Key);
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(2, args.Key);
					Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(3, args.Key);
					Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}
	}
}