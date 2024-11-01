using System;
using System.Collections.Generic;
using Xunit;

namespace Albatross.DateLevel.Test {
	public class TestSetDateLevelEntity {
		[Fact]
		public void Baseline() {
			var list = new List<SpreadSpec>();
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		/// set 1
		[Fact]
		public void Mar100_Jul200_Sep300_Nov300() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Nov1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Sep300() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Aug300() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Aug1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jul31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Aug1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jul300() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jun300() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jun1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.May31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jun1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Mar300() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Feb300() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Feb1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Feb28_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		/// set 2
		[Fact]
		public void Mar100_Jul200_Sep300_Nov400() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Nov1_2022, 400), true);
			list.Sort(Compare); ;

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Oct31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Nov1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(400, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Sep400() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(400, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Aug400() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Aug1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jul31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Aug1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jul400() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jun400() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jun1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.May31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jun1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Mar400() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Feb400() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Feb1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Feb28_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}
		static int Compare(SpreadSpec x, SpreadSpec y) => x.StartDate.CompareTo(y.StartDate);
		/// set 3
		[Fact]
		public void Mar100_Jul200_Sep300_Jun100() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jun1_2022, 100), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Mar100() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Feb100() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Feb1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Feb28_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		///set 4
		[Fact]
		public void Mar100_Jul200_Sep300_Nov200() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Nov1_2022, 200), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Oct31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Nov1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Sep200() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 200), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Aug200() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Aug1_2022, 200), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jul200() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jun200() {
			var list = new List<SpreadSpec>();


			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jun1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.May31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jun1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Mar200() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
					args => {
						Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
						Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
						Assert.Equal(300, args.Value);
					},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Feb200() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Feb1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Feb28_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Jul200() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Feb150_Insert() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Feb1_2022, 150) { EndDate = DateOnlyValues.Mar31_2022 }, true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Mar31_2022, args.EndDate);
					Assert.Equal(150, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Apr1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}
		[Fact]
		public void Mar100_Jul200_Sep300_Apr150_Insert() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Apr1_2022, 150) { EndDate = DateOnlyValues.Jul31_2022 }, true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Mar31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Apr1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Jul31_2022, args.EndDate);
					Assert.Equal(150, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Aug1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}
		[Fact]
		public void Mar100_Jul200_Sep300_Nov400_Apr150_Insert() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Nov1_2022, 400), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Apr1_2022, 150) { EndDate = DateOnlyValues.Sep30_2022 }, true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateOnlyValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Mar31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Apr1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Sep30_2022, args.EndDate);
					Assert.Equal(150, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Oct1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.Oct31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateOnlyValues.Nov1_2022, args.StartDate);
					Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
					Assert.Equal(400, args.Value);
				}
			);
		}
		[Fact]
		public void Mar100_Jul200_Sep300_Insert() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			Assert.Throws<InvalidOperationException>(() =>
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Sep1_2022, 300) {
				EndDate = DateOnlyValues.Nov30_2022,
			}, true));
		}
		[Fact]
		public void Mar100_Jul200_Jun300_Insert() {
			var list = new List<SpreadSpec>();

			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Mar1_2022, 100), true);
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jul1_2022, 200), true);
			Assert.Throws<InvalidOperationException>(() =>
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jun1_2022, 300) {
				EndDate = DateOnlyValues.Jun30_2022,
			}, true));
		}

		[Fact]
		public void SameValue_Insert() {
			var list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Mar31_2022
				},
				new SpreadSpec(1, DateOnlyValues.Apr1_2022, 100) {
					EndDate = DateOnlyValues.Jun30_2022,
				},
				new SpreadSpec(1, DateOnlyValues.Jul1_2022, 100) {
					EndDate = DateOnlyValues.Oct31_2022,
				},
				new SpreadSpec(1, DateOnlyValues.Nov1_2022, 100) {
					EndDate = DateOnlyValues.MaxSqlDate
				},
			};
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.May1_2022, 100), true);
			list.Sort(Compare);
			Assert.Collection(list, args => {
				Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
				Assert.Equal(DateOnlyValues.Mar31_2022, args.EndDate);
			},
			args => {
				Assert.Equal(DateOnlyValues.Apr1_2022, args.StartDate);
				Assert.Equal(DateOnlyValues.Oct31_2022, args.EndDate);
			},
			args => {
				Assert.Equal(DateOnlyValues.Nov1_2022, args.StartDate);
				Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
			});
		}
		[Fact]
		public void SameValue_Insert2() {
			var list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Mar31_2022
				},
				new SpreadSpec(1, DateOnlyValues.Apr1_2022, 100) {
					EndDate = DateOnlyValues.Jun30_2022,
				},
				new SpreadSpec(1, DateOnlyValues.Jul1_2022, 100) {
					EndDate = DateOnlyValues.Oct31_2022,
				},
				new SpreadSpec(1, DateOnlyValues.Nov1_2022, 100) {
					EndDate = DateOnlyValues.MaxSqlDate
				},
			};
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.May1_2022, 100) {
				EndDate = DateOnlyValues.Aug1_2022
			}, true);
			list.Sort(Compare);
			Assert.Collection(list, args => {
				Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
				Assert.Equal(DateOnlyValues.Mar31_2022, args.EndDate);
			},
			args => {
				Assert.Equal(DateOnlyValues.Apr1_2022, args.StartDate);
				Assert.Equal(DateOnlyValues.Oct31_2022, args.EndDate);
			},
			args => {
				Assert.Equal(DateOnlyValues.Nov1_2022, args.StartDate);
				Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
			});
		}
		[Fact]
		public void SameValue_Append() {
			var list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Jan1_2022, 100) {
					EndDate = DateOnlyValues.Mar31_2022
				},
				new SpreadSpec(1, DateOnlyValues.Apr1_2022, 100) {
					EndDate = DateOnlyValues.Jun30_2022,
				},
				new SpreadSpec(1, DateOnlyValues.Jul1_2022, 100) {
					EndDate = DateOnlyValues.Oct31_2022,
				},
				new SpreadSpec(1, DateOnlyValues.Nov1_2022, 100) {
					EndDate = DateOnlyValues.MaxSqlDate
				},
			};
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.May1_2022, 100), false);
			list.Sort(Compare);
			Assert.Collection(list, args => {
				Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
				Assert.Equal(DateOnlyValues.Mar31_2022, args.EndDate);
			},
			args => {
				Assert.Equal(DateOnlyValues.Apr1_2022, args.StartDate);
				Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
			});
		}

		[Fact]
		public void SingleValue_Insert() {
			var list = new List<SpreadSpec> {
				new SpreadSpec(1, DateOnlyValues.Feb1_2022, 1) {
					EndDate = DateOnlyValues.Feb1_2022
				},
				new SpreadSpec(1, DateOnlyValues.Feb2_2022, 2) {
					EndDate = DateOnlyValues.MaxSqlDate
				},
			};
			list.SetDateLevel<SpreadSpec, int>(new SpreadSpec(1, DateOnlyValues.Jan1_2022, 3), true);
			list.Sort(Compare);
			Assert.Collection(list, args => {
				Assert.Equal(3, args.Value);
				Assert.Equal(DateOnlyValues.Jan1_2022, args.StartDate);
				Assert.Equal(DateOnlyValues.Jan31_2022, args.EndDate);
			},
			args => {
				Assert.Equal(1, args.Value);
				Assert.Equal(DateOnlyValues.Feb1_2022, args.StartDate);
				Assert.Equal(DateOnlyValues.Feb1_2022, args.EndDate);
			},
			args => {
				Assert.Equal(2, args.Value);
				Assert.Equal(DateOnlyValues.Feb2_2022, args.StartDate);
				Assert.Equal(DateOnlyValues.MaxSqlDate, args.EndDate);
			});
		}
	}
}