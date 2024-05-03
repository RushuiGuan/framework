using System;

namespace Albatross.DateLevel.Test {
	public class MyDateLevelValue: DateLevelEntity<int> {
		public int V1{get;set; }
		public int V2{get;set; }
		public override int Key { get; }
		public MyDateLevelValue(DateOnly startDate, int key, int v1, int v2) : base(startDate) {
			this.Key = key;
			this.V1 = v1;
			this.V2	= v2;
		}
		public override bool HasSameValue(DateLevelEntity src) {
			if (src is MyDateLevelValue my) {
				return this.V1 == my.V1 && this.V2 == my.V2;
			} else {
				return false;
			}
		}

		public override object Clone() {
			return new MyDateLevelValue(StartDate, Key, V1, V2) {
				EndDate = this.EndDate,
			};
		}
	}

	public class TestUpdateDateLevelEntity {
	}
}
