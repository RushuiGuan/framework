using Albatross.Hosting.Test;
using Sample.EFCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Albatross.DateLevel.Test {
	public class TestGetDateTimeLevelEntityByDate {
		[Fact]
		public void No_Row() {
			List<ContractSpec> list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Jan1_2022, 100) {
					EndDate = DateTimeValues.Jan31_2022
				}
			};
			var result = list.GetDateLevelEntityByDate(new DateTime(2022, 2, 1));
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void Single_Row() {
			List<ContractSpec> list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Jan1_2022, 100) {
					EndDate = DateTimeValues.Jan31_2022
				}
			};
			var result = list.GetDateLevelEntityByDate(new DateTime(2022, 1, 1));
			Assert.NotNull(result);
			Assert.Single(result);
			Assert.Equal(DateTimeValues.Jan1_2022, result.First().StartDate);
			Assert.Equal(DateTimeValues.Jan31_2022, result.First().EndDate);
		}

		[Fact]
		public void Single_Row_Max_Date() {
			List<ContractSpec> list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Jan1_2022, 100) {
					EndDate = DateTimeValues.Jan31_2022
				},
				new ContractSpec(1, DateTimeValues.Feb1_2022, 100) {
					EndDate = DateTimeValues.Feb28_2022
				},
				new ContractSpec(1, DateTimeValues.Mar1_2022, 100) {
					EndDate = DateTimeValues.Mar31_2022
				},
				new ContractSpec(1, DateTimeValues.Apr1_2022, 100) 
			};
			var result = list.GetDateLevelEntityByDate(new DateTime(2022, 4, 10));
			Assert.NotNull(result);
			Assert.Single(result);
			Assert.Equal(DateTimeValues.Apr1_2022, result.First().StartDate);
			Assert.Equal(DateTimeLevelEntity.MaxEndDate, result.First().EndDate);
		}

		[Fact]
		public void Multiple_Rows() {
			List<ContractSpec> list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Jan1_2022, 100) {
					EndDate = DateTimeValues.Jan31_2022
				},
				new ContractSpec(1, DateTimeValues.Feb1_2022, 100) {
					EndDate = DateTimeValues.Feb28_2022
				},
				new ContractSpec(1, DateTimeValues.Mar1_2022, 100) {
					EndDate = DateTimeValues.Mar31_2022
				},
				new ContractSpec(1, DateTimeValues.Apr1_2022, 100) {
					EndDate = DateTimeValues.Apr30_2022
				},
				new ContractSpec(2, DateTimeValues.Apr1_2022, 100)
			};
			var result = list.GetDateLevelEntityByDate(new DateTime(2022, 4, 10));
			Assert.NotNull(result);
			Assert.Equal(2, result.Count());
			Assert.Equal(DateTimeValues.Apr1_2022, result.First().StartDate);
			Assert.Equal(DateTimeValues.Apr30_2022, result.First().EndDate);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(DateTimeValues.Apr1_2022, result.ElementAt(1).StartDate);
			Assert.Equal(DateTimeLevelEntity.MaxEndDate, result.ElementAt(1).EndDate);
			Assert.Equal(2, result.ElementAt(1).Key);
		}
	}
}
