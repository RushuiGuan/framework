using Albatross.EFCore.DateLevel;
using Albatross.Hosting.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

#if NET8_0
namespace Albatross.EFCore.Test {
	public class TestGetDateLevelEntityByDateRange {
		[Fact]
		public void No_Row() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan31_2022
				}
			};
			var result = list.GetDateLevelEntityByDateRange(new DateOnly(2022, 2, 1), new DateOnly(2022, 2, 10));
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void Single_Row() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan31_2022
				},
				new TickSize(1, DateOnlyValues.Feb1_2022, 150) {
					EndDate = DateOnlyValues.Mar31_2022
				}
			};
			var result = list.GetDateLevelEntityByDateRange(new DateOnly(2022, 1, 5), new DateOnly(2022, 1, 15));
			Assert.NotNull(result);
			Assert.Single(result);
			Assert.Equal(DateOnlyValues.Jan1_2022, result.First().StartDate);
			Assert.Equal(DateOnlyValues.Jan31_2022, result.First().EndDate);
		}

		[Fact]
		public void Multiple_Rows() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan31_2022
				},
				new TickSize(1, DateOnlyValues.Feb1_2022, 100) {
					EndDate = DateOnlyValues.Feb28_2022
				},
				new TickSize(1, DateOnlyValues.Mar1_2022, 100) {
					EndDate = DateOnlyValues.Mar31_2022
				},
				new TickSize(2, DateOnlyValues.Apr1_2022, 100)
			};
			var result = list.GetDateLevelEntityByDateRange(new DateOnly(2022, 1, 31), new DateOnly(2022, 3, 7));
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
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan31_2022
				},
				new TickSize(1, DateOnlyValues.Feb1_2022, 100) {
					EndDate = DateOnlyValues.Feb28_2022
				},
				new TickSize(1, DateOnlyValues.Mar1_2022, 100) {
					EndDate = new DateOnly(2022, 3, 15)
				},
				new TickSize(1, new DateOnly(2022, 3, 16), 100) {
					EndDate = DateOnlyValues.Mar31_2022
				},
				new TickSize(2, DateOnlyValues.Apr1_2022, 100)
			};
			var result = list.GetDateLevelEntityByDateRange(new DateOnly(2022, 2, 21), new DateOnly(2022, 4, 30));
			Assert.NotNull(result);
			Assert.Equal(4, result.Count());
			Assert.Equal(DateOnlyValues.Feb1_2022, result.First().StartDate);
			Assert.Equal(DateOnlyValues.Feb28_2022, result.First().EndDate);
			Assert.Equal(DateOnlyValues.Mar1_2022, result.ElementAt(1).StartDate);
			Assert.Equal(new DateOnly(2022, 3, 15), result.ElementAt(1).EndDate);
			Assert.Equal(new DateOnly(2022, 3, 16), result.ElementAt(2).StartDate);
			Assert.Equal(DateOnlyValues.Mar31_2022, result.ElementAt(2).EndDate);
			Assert.Equal(DateOnlyValues.Apr1_2022, result.ElementAt(3).StartDate);
			Assert.Equal(DateOnlyValues.MaxSqlDate, result.ElementAt(3).EndDate);
		}

	}
}
#endif