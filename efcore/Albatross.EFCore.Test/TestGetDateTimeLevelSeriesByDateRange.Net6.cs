using Albatross.EFCore.DateLevel;
using Albatross.Hosting.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Albatross.EFCore.Test {
	public class TestGetDateTimeLevelEntityByDateRange {
		[Fact]
		public void No_Row() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, DateTimeValues.Jan1_2022, 100) {
					EndDate = DateTimeValues.Jan31_2022
				}
			};
			var result = list.GetDateLevelEntityByDateRange(new DateTime(2022, 2, 1), new DateTime(2022, 2, 10));
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void Single_Row() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, DateTimeValues.Jan1_2022, 100) {
					EndDate = DateTimeValues.Jan31_2022
				},
				new TickSize(1, DateTimeValues.Feb1_2022, 150) {
					EndDate = DateTimeValues.Mar31_2022
				}
			};
			var result = list.GetDateLevelEntityByDateRange(new DateTime(2022, 1, 5), new DateTime(2022, 1, 15));
			Assert.NotNull(result);
			Assert.Single(result);
			Assert.Equal(DateTimeValues.Jan1_2022, result.First().StartDate);
			Assert.Equal(DateTimeValues.Jan31_2022, result.First().EndDate);
		}

		[Fact]
		public void Multiple_Rows() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, DateTimeValues.Jan1_2022, 100) {
					EndDate = DateTimeValues.Jan31_2022
				},
				new TickSize(1, DateTimeValues.Feb1_2022, 100) {
					EndDate = DateTimeValues.Feb28_2022
				},
				new TickSize(1, DateTimeValues.Mar1_2022, 100) {
					EndDate = DateTimeValues.Mar31_2022
				},
				new TickSize(2, DateTimeValues.Apr1_2022, 100)
			};
			var result = list.GetDateLevelEntityByDateRange(new DateTime(2022, 1, 31), new DateTime(2022, 3, 7));
			Assert.NotNull(result);
			Assert.Equal(3, result.Count());
			Assert.Equal(DateTimeValues.Jan1_2022, result.First().StartDate);
			Assert.Equal(DateTimeValues.Jan31_2022, result.First().EndDate);
			Assert.Equal(DateTimeValues.Feb1_2022, result.ElementAt(1).StartDate);
			Assert.Equal(DateTimeValues.Feb28_2022, result.ElementAt(1).EndDate);
			Assert.Equal(DateTimeValues.Mar1_2022, result.ElementAt(2).StartDate);
			Assert.Equal(DateTimeValues.Mar31_2022, result.ElementAt(2).EndDate);
		}

		[Fact]
		public void Multiple_Rows_With_Max_Date() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, DateTimeValues.Jan1_2022, 100) {
					EndDate = DateTimeValues.Jan31_2022
				},
				new TickSize(1, DateTimeValues.Feb1_2022, 100) {
					EndDate = DateTimeValues.Feb28_2022
				},
				new TickSize(1, DateTimeValues.Mar1_2022, 100) {
					EndDate = new DateTime(2022, 3, 15)
				},
				new TickSize(1, new DateTime(2022, 3, 16), 100) {
					EndDate = DateTimeValues.Mar31_2022
				},
				new TickSize(2, DateTimeValues.Apr1_2022, 100)
			};
			var result = list.GetDateLevelEntityByDateRange(new DateTime(2022, 2, 21), new DateTime(2022, 4, 30));
			Assert.NotNull(result);
			Assert.Equal(4, result.Count());
			Assert.Equal(DateTimeValues.Feb1_2022, result.First().StartDate);
			Assert.Equal(DateTimeValues.Feb28_2022, result.First().EndDate);
			Assert.Equal(DateTimeValues.Mar1_2022, result.ElementAt(1).StartDate);
			Assert.Equal(new DateTime(2022, 3, 15), result.ElementAt(1).EndDate);
			Assert.Equal(new DateTime(2022, 3, 16), result.ElementAt(2).StartDate);
			Assert.Equal(DateTimeValues.Mar31_2022, result.ElementAt(2).EndDate);
			Assert.Equal(DateTimeValues.Apr1_2022, result.ElementAt(3).StartDate);
			Assert.Equal(DateTimeValues.MaxSqlDate, result.ElementAt(3).EndDate);
		}

	}
}
