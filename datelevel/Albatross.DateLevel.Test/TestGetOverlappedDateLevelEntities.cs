using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Albatross.DateLevel.Test {
	public class TestTestGetOverlappedDateLevelEntities {
		static List<MyDateLevelValue> BaseLine() => new List<MyDateLevelValue> {
				new MyDateLevelValue(new DateOnly(2024, 4, 1), 400, 4000){
					EndDate = new DateOnly(2024, 4, 30)
				},
				new MyDateLevelValue(new DateOnly(2024, 5, 1), 500,5000){
					EndDate = new DateOnly(2024, 5, 31)
				},
				new MyDateLevelValue(new DateOnly(2024, 6, 1), 600,6000){
					EndDate = new DateOnly(2024, 6, 30)
				},
				new MyDateLevelValue(new DateOnly(2024, 7, 1), 700,7000){
					EndDate = new DateOnly(2024, 7, 31)
				},
				new MyDateLevelValue(new DateOnly(2024, 8, 1), 800,8000)
			};

		[Fact]
		public void Test_1() {
			var list = BaseLine();
			var items = list.GetOverlappedDateLevelEntities(9999, new DateOnly(2024, 4, 1), null);
			Assert.Collection(items,
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
				});
		}
		[Fact]
		public void Test_2() {
			var list = BaseLine();
			var items = list.GetOverlappedDateLevelEntities(9999, new DateOnly(2024, 4, 2), null);
			Assert.Collection(items,
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
				});
		}
		[Fact]
		public void Test_3() {
			var list = BaseLine();
			var items = list.GetOverlappedDateLevelEntities(9999, new DateOnly(2024, 4, 2), new DateOnly(2024, 4, 2));
			Assert.Collection(items,
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
				});
		}
		[Fact]
		public void Test_4() {
			var list = BaseLine();
			var items = list.GetOverlappedDateLevelEntities(9999, new DateOnly(2024, 4, 2), new DateOnly(2024, 5, 2));
			Assert.Collection(items,
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 5, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 5, 31), x.EndDate);
				});
		}
		[Fact]
		public void Test_5() {
			var list = BaseLine();
			var items = list.GetOverlappedDateLevelEntities(9999, new DateOnly(2024, 9, 1), null);
			Assert.Collection(items,
				x => {
					Assert.Equal(new DateOnly(2024, 8, 1), x.StartDate);
					Assert.Equal(new DateOnly(9999, 12, 31), x.EndDate);
				});
		}
	}
}