using Albatross.Hosting.Test;
using Albatross.Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Albatross.Repository.Test {
	public class TestGetDateLevelEntityByDate {
		[Fact]
		public void No_Row() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Jan1_2022, 100) {
					EndDate = Values.Jan31_2022
				}
			};
			var result = list.GetDateLevelEntityByDate(new DateTime(2022, 2, 1));
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void Single_Row() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Jan1_2022, 100) {
					EndDate = Values.Jan31_2022
				}
			};
			var result = list.GetDateLevelEntityByDate(new DateTime(2022, 1, 1));
			Assert.NotNull(result);
			Assert.Single(result);
			Assert.Equal(Values.Jan1_2022, result.First().StartDate);
			Assert.Equal(Values.Jan31_2022, result.First().EndDate);
		}

		[Fact]
		public void Single_Row_Max_Date() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Jan1_2022, 100) {
					EndDate = Values.Jan31_2022
				},
				new TickSize(1, Values.Feb1_2022, 100) {
					EndDate = Values.Feb28_2022
				},
				new TickSize(1, Values.Mar1_2022, 100) {
					EndDate = Values.Mar31_2022
				},
				new TickSize(1, Values.Apr1_2022, 100) 
			};
			var result = list.GetDateLevelEntityByDate(new DateTime(2022, 4, 10));
			Assert.NotNull(result);
			Assert.Single(result);
			Assert.Equal(Values.Apr1_2022, result.First().StartDate);
			Assert.Equal(DateLevelEntity.MaxEndDate, result.First().EndDate);
		}

		[Fact]
		public void Multiple_Rows() {
			List<TickSize> list = new List<TickSize> {
				new TickSize(1, Values.Jan1_2022, 100) {
					EndDate = Values.Jan31_2022
				},
				new TickSize(1, Values.Feb1_2022, 100) {
					EndDate = Values.Feb28_2022
				},
				new TickSize(1, Values.Mar1_2022, 100) {
					EndDate = Values.Mar31_2022
				},
				new TickSize(1, Values.Apr1_2022, 100) {
					EndDate = Values.Apr30_2022
				},
				new TickSize(2, Values.Apr1_2022, 100)
			};
			var result = list.GetDateLevelEntityByDate(new DateTime(2022, 4, 10));
			Assert.NotNull(result);
			Assert.Equal(2, result.Count());
			Assert.Equal(Values.Apr1_2022, result.First().StartDate);
			Assert.Equal(Values.Apr30_2022, result.First().EndDate);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(Values.Apr1_2022, result.ElementAt(1).StartDate);
			Assert.Equal(DateLevelEntity.MaxEndDate, result.ElementAt(1).EndDate);
			Assert.Equal(2, result.ElementAt(1).Key);
		}
	}
}
