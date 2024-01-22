using Albatross.EFCore.DateLevel;
using Albatross.Hosting.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

#if NET8_0
namespace Albatross.EFCore.Test {
	public class TestGetDateLevelEntityByDate {
		[Fact]
		public void No_Row() {
			List<TickSize2> list = new List<TickSize2> {
				new TickSize2(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan31_2022
				}
			};
			var result = list.GetDateLevelEntityByDate(new DateOnly(2022, 2, 1));
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void Single_Row() {
			List<TickSize2> list = new List<TickSize2> {
				new TickSize2(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan31_2022
				}
			};
			var result = list.GetDateLevelEntityByDate(new DateOnly(2022, 1, 1));
			Assert.NotNull(result);
			Assert.Single(result);
			Assert.Equal(DateOnlyValues.Jan1_2022, result.First().StartDate);
			Assert.Equal(DateOnlyValues.Jan31_2022, result.First().EndDate);
		}

		[Fact]
		public void Single_Row_Max_Date() {
			List<TickSize2> list = new List<TickSize2> {
				new TickSize2(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan31_2022
				},
				new TickSize2(1, DateOnlyValues.Feb1_2022, 100) {
					EndDate = DateOnlyValues.Feb28_2022
				},
				new TickSize2(1, DateOnlyValues.Mar1_2022, 100) {
					EndDate = DateOnlyValues.Mar31_2022
				},
				new TickSize2(1, DateOnlyValues.Apr1_2022, 100) 
			};
			var result = list.GetDateLevelEntityByDate(new DateOnly(2022, 4, 10));
			Assert.NotNull(result);
			Assert.Single(result);
			Assert.Equal(DateOnlyValues.Apr1_2022, result.First().StartDate);
			Assert.Equal(DateLevelEntity.MaxEndDate, result.First().EndDate);
		}

		[Fact]
		public void Multiple_Rows() {
			List<TickSize2> list = new List<TickSize2> {
				new TickSize2(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Jan31_2022
				},
				new TickSize2(1, DateOnlyValues.Feb1_2022, 100) {
					EndDate = DateOnlyValues.Feb28_2022
				},
				new TickSize2(1, DateOnlyValues.Mar1_2022, 100) {
					EndDate = DateOnlyValues.Mar31_2022
				},
				new TickSize2(1, DateOnlyValues.Apr1_2022, 100) {
					EndDate = DateOnlyValues.Apr30_2022
				},
				new TickSize2(2, DateOnlyValues.Apr1_2022, 100)
			};
			var result = list.GetDateLevelEntityByDate(new DateOnly(2022, 4, 10));
			Assert.NotNull(result);
			Assert.Equal(2, result.Count());
			Assert.Equal(DateOnlyValues.Apr1_2022, result.First().StartDate);
			Assert.Equal(DateOnlyValues.Apr30_2022, result.First().EndDate);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(DateOnlyValues.Apr1_2022, result.ElementAt(1).StartDate);
			Assert.Equal(DateLevelEntity.MaxEndDate, result.ElementAt(1).EndDate);
			Assert.Equal(2, result.ElementAt(1).Key);
		}
	}
}
#endif