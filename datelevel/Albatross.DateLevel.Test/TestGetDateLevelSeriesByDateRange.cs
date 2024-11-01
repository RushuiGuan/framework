using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Albatross.DateLevel.Test {
	public class TestGetDateLevelEntityByDateRange {
		[Fact]
		public void No_Row() {
			List<SpreadSpec> list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan31_2022
				}
			};
			var result = list.GetOverlappedDateLevelEntities(1, new DateOnly(2022, 2, 1), new DateOnly(2022, 2, 10));
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void Single_Row() {
			List<SpreadSpec> list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan31_2022
				},
				new SpreadSpec(1, DateOnlyValues.Feb1_2022, 150) {
					EndDate = DateOnlyValues.Mar31_2022
				}
			};
			var result = list.GetOverlappedDateLevelEntities(1, new DateOnly(2022, 1, 5), new DateOnly(2022, 1, 15));
			Assert.NotNull(result);
			Assert.Single(result);
			Assert.Equal(DateOnlyValues.Jan1_2022, result.First().StartDate);
			Assert.Equal(DateOnlyValues.Jan31_2022, result.First().EndDate);
		}

		[Fact]
		public void Multiple_Rows() {
			List<SpreadSpec> list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan31_2022
				},
				new SpreadSpec(1, DateOnlyValues.Feb1_2022, 100) {
					EndDate = DateOnlyValues.Feb28_2022
				},
				new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100) {
					EndDate = DateOnlyValues.Mar31_2022
				},
				new SpreadSpec(2, DateOnlyValues.Apr1_2022, 100)
			};
			var result = list.GetOverlappedDateLevelEntities(1, new DateOnly(2022, 1, 31), new DateOnly(2022, 3, 7));
			Assert.NotNull(result);
			Assert.Equal(3, result.Count());
			Assert.Equal(DateOnlyValues.Jan1_2022, result.First().StartDate);
			Assert.Equal(DateOnlyValues.Jan31_2022, result.First().EndDate);
			Assert.Equal(DateOnlyValues.Feb1_2022, result.ElementAt(1).StartDate);
			Assert.Equal(DateOnlyValues.Feb28_2022, result.ElementAt(1).EndDate);
			Assert.Equal(DateOnlyValues.Mar1_2022, result.ElementAt(2).StartDate);
			Assert.Equal(DateOnlyValues.Mar31_2022, result.ElementAt(2).EndDate);
		}

		[Fact]
		public void Multiple_Rows_With_Max_Date() {
			List<SpreadSpec> list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan31_2022
				},
				new SpreadSpec(1, DateOnlyValues.Feb1_2022, 100) {
					EndDate = DateOnlyValues.Feb28_2022
				},
				new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100) {
					EndDate = new DateOnly(2022, 3, 15)
				},
				new SpreadSpec(1, new DateOnly(2022, 3, 16), 100) {
					EndDate = DateOnlyValues.Mar31_2022
				},
				new SpreadSpec(2, DateOnlyValues.Apr1_2022, 100)
			};
			var result = list.GetOverlappedDateLevelEntities(1, new DateOnly(2022, 2, 21), new DateOnly(2022, 4, 30));
			Assert.NotNull(result);
			Assert.Collection(result, x => {
				Assert.Equal(1, x.Key);
				Assert.Equal(DateOnlyValues.Feb1_2022, x.StartDate);
				Assert.Equal(DateOnlyValues.Feb28_2022, x.EndDate);
			}, x => {
				Assert.Equal(1, x.Key);
				Assert.Equal(DateOnlyValues.Mar1_2022, x.StartDate);
				Assert.Equal(new DateOnly(2022, 3, 15), x.EndDate);
			}, x => {
				Assert.Equal(1, x.Key);
				Assert.Equal(new DateOnly(2022, 3, 16), x.StartDate);
				Assert.Equal(DateOnlyValues.Mar31_2022, x.EndDate);
			});
		}

	}
}