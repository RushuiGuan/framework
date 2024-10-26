using Albatross.DateLevel.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Albatross.DateLevel.Test {
	public class MyDateLevelValue : DateLevelEntity<int> {
		public int V1 { get; set; }
		public int V2 { get; set; }
		public override int Key => 9999;
		public MyDateLevelValue(DateOnly startDate, int v1, int v2) : base(startDate) {
			this.V1 = v1;
			this.V2 = v2;
		}
		public override bool HasSameValue(DateLevelEntity src) {
			if (src is MyDateLevelValue my) {
				return this.Key == my.Key && this.V1 == my.V1 && this.V2 == my.V2;
			} else {
				return false;
			}
		}

		public override object Clone() {
			return new MyDateLevelValue(StartDate, V1, V2) {
				EndDate = this.EndDate,
			};
		}
	}

	public class TestUpdateDateLevelEntity {
		[Fact]
		public void Test_ChangedOverlapCurrent() {
			var list = new List<MyDateLevelValue> {
				new MyDateLevelValue(new DateOnly(2024, 4, 1), 100, 200){
					EndDate = new DateOnly(2024, 4, 30)
				},
			};
			list.UpdateDateLevel(x => x.V1 = 999, new DateOnly(2024, 3, 1), new DateOnly(2024, 5, 1), false);
			Assert.Collection(list,
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
					Assert.Equal(999, x.V1);
					Assert.Equal(200, x.V2);
				});
		}

		[Fact]
		public void Test_CurrentOverlapChanged_SameStart_SameEnd() {
			// same start and end date
			var list = new List<MyDateLevelValue> {
				new MyDateLevelValue(new DateOnly(2024, 4, 1), 100, 200){
					EndDate = new DateOnly(2024, 4, 30)
				},
			};
			list.UpdateDateLevel(x => x.V1 = 999, new DateOnly(2024, 4, 1), new DateOnly(2024, 4, 30), false);
			Assert.Collection(list,
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
					Assert.Equal(999, x.V1);
					Assert.Equal(200, x.V2);
				});
		}

		[Fact]
		public void Test_CurrentOverlapChanged_SameStart_DiffEnd() {
			// same start date, diff end date
			var list = new List<MyDateLevelValue> {
				new MyDateLevelValue(new DateOnly(2024, 4, 1), 100, 200){
					EndDate = new DateOnly(2024, 4, 30)
				},
			};
			list.UpdateDateLevel(x => x.V1 = 999, new DateOnly(2024, 4, 1), new DateOnly(2024, 4, 20), false);
			Assert.Collection(list.OrderBy(x => x.StartDate),
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 20), x.EndDate);
					Assert.Equal(999, x.V1);
					Assert.Equal(200, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 4, 21), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
					Assert.Equal(100, x.V1);
					Assert.Equal(200, x.V2);
				});
		}

		[Fact]
		public void Test_CurrentOverlapChanged_DiffStart_SameEnd() {
			// diff start date, same end date
			var list = new List<MyDateLevelValue> {
				new MyDateLevelValue(new DateOnly(2024, 4, 1), 100, 200){
					EndDate = new DateOnly(2024, 4, 30)
				},
			};
			list.UpdateDateLevel(x => x.V1 = 999, new DateOnly(2024, 4, 10), new DateOnly(2024, 4, 30), false);
			Assert.Collection(list.OrderBy(x => x.StartDate),
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 9), x.EndDate);
					Assert.Equal(100, x.V1);
					Assert.Equal(200, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 4, 10), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
					Assert.Equal(999, x.V1);
					Assert.Equal(200, x.V2);
				});
		}

		[Fact]
		public void Test_CurrentOverlapChanged_DiffStart_DiffEnd() {
			// diff start date and diff end date
			var list = new List<MyDateLevelValue> {
				new MyDateLevelValue(new DateOnly(2024, 4, 1), 100, 200){
					EndDate = new DateOnly(2024, 4, 30)
				},
			};
			list.UpdateDateLevel(x => x.V1 = 999, new DateOnly(2024, 4, 10), new DateOnly(2024, 4, 20), false);
			Assert.Collection(list.OrderBy(x => x.StartDate),
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 9), x.EndDate);
					Assert.Equal(100, x.V1);
					Assert.Equal(200, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 4, 10), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 20), x.EndDate);
					Assert.Equal(999, x.V1);
					Assert.Equal(200, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 4, 21), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
					Assert.Equal(100, x.V1);
					Assert.Equal(200, x.V2);
				});

		}

		[Fact]
		public void Test_ChangedOverlapCurrentStart_1() {
			var list = new List<MyDateLevelValue> {
				new MyDateLevelValue(new DateOnly(2024, 4, 1), 100, 200){
					EndDate = new DateOnly(2024, 4, 30)
				},
			};
			list.UpdateDateLevel(x => x.V1 = 999, new DateOnly(2024, 3, 1), new DateOnly(2024, 4, 20), false);
			Assert.Collection(list.OrderBy(x => x.StartDate),
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 20), x.EndDate);
					Assert.Equal(999, x.V1);
					Assert.Equal(200, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 4, 21), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
					Assert.Equal(100, x.V1);
					Assert.Equal(200, x.V2);
				});
		}

		[Fact]
		public void Test_ChangedOverlapCurrentStart_2() {
			var list = new List<MyDateLevelValue> {
				new MyDateLevelValue(new DateOnly(2024, 4, 1), 100, 200){
					EndDate = new DateOnly(2024, 4, 30)
				},
			};
			list.UpdateDateLevel(x => x.V1 = 999, new DateOnly(2024, 3, 1), new DateOnly(2024, 4, 1), false);
			Assert.Collection(list.OrderBy(x => x.StartDate),
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 1), x.EndDate);
					Assert.Equal(999, x.V1);
					Assert.Equal(200, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 4, 2), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
					Assert.Equal(100, x.V1);
					Assert.Equal(200, x.V2);
				});
		}

		[Fact]
		public void Test_ChangedOverlapCurrentEnd_1() {
			var list = new List<MyDateLevelValue> {
				new MyDateLevelValue(new DateOnly(2024, 4, 1), 100, 200){
					EndDate = new DateOnly(2024, 4, 30)
				},
			};
			list.UpdateDateLevel(x => x.V1 = 999, new DateOnly(2024, 4, 20), new DateOnly(2024, 5, 10), false);
			Assert.Collection(list.OrderBy(x => x.StartDate),
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 19), x.EndDate);
					Assert.Equal(100, x.V1);
					Assert.Equal(200, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 4, 20), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
					Assert.Equal(999, x.V1);
					Assert.Equal(200, x.V2);
				});
		}

		[Fact]
		public void Test_ChangedOverlapCurrentEnd_2() {
			var list = new List<MyDateLevelValue> {
				new MyDateLevelValue(new DateOnly(2024, 4, 1), 100, 200){
					EndDate = new DateOnly(2024, 4, 30)
				},
			};
			list.UpdateDateLevel(x => x.V1 = 999, new DateOnly(2024, 4, 30), new DateOnly(2024, 5, 10), false);
			Assert.Collection(list.OrderBy(x => x.StartDate),
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 29), x.EndDate);
					Assert.Equal(100, x.V1);
					Assert.Equal(200, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 4, 30), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
					Assert.Equal(999, x.V1);
					Assert.Equal(200, x.V2);
				});
		}


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
		public void Test_UpdateWithStartDateOnly() {
			var list = BaseLine();
			list.UpdateDateLevel(x => x.V1 += 1, new DateOnly(2024, 4, 15), null, false);
			Assert.Collection(list.OrderBy(x => x.StartDate),
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 14), x.EndDate);
					Assert.Equal(400, x.V1);
					Assert.Equal(4000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 4, 15), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
					Assert.Equal(401, x.V1);
					Assert.Equal(4000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 5, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 5, 31), x.EndDate);
					Assert.Equal(500, x.V1);
					Assert.Equal(5000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 6, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 6, 30), x.EndDate);
					Assert.Equal(600, x.V1);
					Assert.Equal(6000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 7, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 7, 31), x.EndDate);
					Assert.Equal(700, x.V1);
					Assert.Equal(7000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 8, 1), x.StartDate);
					Assert.Equal(new DateOnly(9999, 12, 31), x.EndDate);
					Assert.Equal(800, x.V1);
					Assert.Equal(8000, x.V2);
				});
		}


		[Fact]
		public void Test_AllUserCase_1() {
			var list = BaseLine();
			list.UpdateDateLevel(x => x.V1 += 1, new DateOnly(2024, 4, 15), new DateOnly(2024, 5, 15), false);
			Assert.Collection(list.OrderBy(x => x.StartDate),
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 14), x.EndDate);
					Assert.Equal(400, x.V1);
					Assert.Equal(4000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 4, 15), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
					Assert.Equal(401, x.V1);
					Assert.Equal(4000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 5, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 5, 15), x.EndDate);
					Assert.Equal(501, x.V1);
					Assert.Equal(5000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 5, 16), x.StartDate);
					Assert.Equal(new DateOnly(2024, 5, 31), x.EndDate);
					Assert.Equal(500, x.V1);
					Assert.Equal(5000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 6, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 6, 30), x.EndDate);
					Assert.Equal(600, x.V1);
					Assert.Equal(6000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 7, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 7, 31), x.EndDate);
					Assert.Equal(700, x.V1);
					Assert.Equal(7000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 8, 1), x.StartDate);
					Assert.Equal(new DateOnly(9999, 12, 31), x.EndDate);
					Assert.Equal(800, x.V1);
					Assert.Equal(8000, x.V2);
				});
		}

		[Fact]
		public void Test_AllUserCase_2() {
			var list = BaseLine();
			list.UpdateDateLevel(x => x.V1 += 1, new DateOnly(2024, 4, 15), new DateOnly(2024, 7, 15), false);
			Assert.Collection(list.OrderBy(x => x.StartDate),
				x => {
					Assert.Equal(new DateOnly(2024, 4, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 14), x.EndDate);
					Assert.Equal(400, x.V1);
					Assert.Equal(4000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 4, 15), x.StartDate);
					Assert.Equal(new DateOnly(2024, 4, 30), x.EndDate);
					Assert.Equal(401, x.V1);
					Assert.Equal(4000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 5, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 5, 31), x.EndDate);
					Assert.Equal(501, x.V1);
					Assert.Equal(5000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 6, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 6, 30), x.EndDate);
					Assert.Equal(601, x.V1);
					Assert.Equal(6000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 7, 1), x.StartDate);
					Assert.Equal(new DateOnly(2024, 7, 15), x.EndDate);
					Assert.Equal(701, x.V1);
					Assert.Equal(7000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 7, 16), x.StartDate);
					Assert.Equal(new DateOnly(2024, 7, 31), x.EndDate);
					Assert.Equal(700, x.V1);
					Assert.Equal(7000, x.V2);
				},
				x => {
					Assert.Equal(new DateOnly(2024, 8, 1), x.StartDate);
					Assert.Equal(new DateOnly(9999, 12, 31), x.EndDate);
					Assert.Equal(800, x.V1);
					Assert.Equal(8000, x.V2);
				});
		}
	}
}