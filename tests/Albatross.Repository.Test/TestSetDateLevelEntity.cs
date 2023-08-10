using Albatross.Hosting.Test;
using System;
using System.Collections.Generic;
using Xunit;


namespace Albatross.Repository.Test {
	public class TestSetDateLevelEntity {
		[Fact]
		public void Baseline() {
			var list = new List<TickSize>();
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		/// set 1
		[Fact]
		public void Mar100_Jul200_Sep300_Nov300() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Nov1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Sep300() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Aug300() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Aug1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Jul31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Aug1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jul300() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jun300() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jun1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.May31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jun1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Mar300() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Feb300() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Feb1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Feb1_2022, args.StartDate);
					Assert.Equal(Values.Feb28_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		/// set 2
		[Fact]
		public void Mar100_Jul200_Sep300_Nov400() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Nov1_2022, 400), true);
			list.Sort(Compare);;

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.Oct31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(Values.Nov1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(400, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Sep400() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(400, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Aug400() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Aug1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Jul31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Aug1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jul400() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jun400() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jun1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.May31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jun1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Mar400() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Feb400() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Feb1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Feb1_2022, args.StartDate);
					Assert.Equal(Values.Feb28_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}
		static int Compare(TickSize x, TickSize y) => x.StartDate.CompareTo(y.StartDate);
		/// set 3
		[Fact]
		public void Mar100_Jul200_Sep300_Jun100() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jun1_2022, 100), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Mar100() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Feb100() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Feb1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Feb1_2022, args.StartDate);
					Assert.Equal(Values.Feb28_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		///set 4
		[Fact]
		public void Mar100_Jul200_Sep300_Nov200() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Nov1_2022, 200), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.Oct31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(Values.Nov1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Sep200() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 200), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Aug200() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Aug1_2022, 200), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jul200() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jun200() {
			var list = new List<TickSize>();


			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jun1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.May31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jun1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Mar200() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
					args => {
						Assert.Equal(Values.Mar1_2022, args.StartDate);
						Assert.Equal(Values.Jun30_2022, args.EndDate);
						Assert.Equal(300, args.Value);
					},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Feb200() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Feb1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Feb1_2022, args.StartDate);
					Assert.Equal(Values.Feb28_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Jul200() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Feb150_Insert() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Feb1_2022, 150) { EndDate = Values.Mar31_2022}, true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Feb1_2022, args.StartDate);
					Assert.Equal(Values.Mar31_2022, args.EndDate);
					Assert.Equal(150, args.Value);
				},
				args => {
					Assert.Equal(Values.Apr1_2022, args.StartDate);
					Assert.Equal(Values.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Jul1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate); 
					Assert.Equal(200, args.Value);
				}
			);
		}
		[Fact]
		public void Mar100_Jul200_Sep300_Apr150_Insert() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Apr1_2022, 150) { EndDate = Values.Jul31_2022 }, true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Mar31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Apr1_2022, args.StartDate);
					Assert.Equal(Values.Jul31_2022, args.EndDate);
					Assert.Equal(150, args.Value);
				},
				args => {
					Assert.Equal(Values.Aug1_2022, args.StartDate);
					Assert.Equal(Values.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(Values.Sep1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}
		[Fact]
		public void Mar100_Jul200_Sep300_Nov400_Apr150_Insert() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Nov1_2022, 400), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Apr1_2022, 150) { EndDate = Values.Sep30_2022 }, true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Mar31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Apr1_2022, args.StartDate);
					Assert.Equal(Values.Sep30_2022, args.EndDate);
					Assert.Equal(150, args.Value);
				},
				args => {
					Assert.Equal(Values.Oct1_2022, args.StartDate);
					Assert.Equal(Values.Oct31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(Values.Nov1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(400, args.Value);
				}
			);
		}
		[Fact]
		public void Mar100_Jul200_Sep300_Insert() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			Assert.Throws<InvalidOperationException>(() =>
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300) {
				EndDate = Values.Nov30_2022,
			}, true));
		}
		[Fact]
		public void Mar100_Jul200_Jun300_Insert() {
			var list = new List<TickSize>();

			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), true);
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), true);
			Assert.Throws<InvalidOperationException>(() =>
			list.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jun1_2022, 300) {
				EndDate = Values.Jun30_2022,
			}, true));
		}

		[Fact]
		public void TestClone() {
			TickSize ts = new (1, Values.Mar1_2022, 100);
			TickSize testTick = (TickSize) ts.Clone();
			Assert.Equal(ts.Market, testTick.Market);
			Assert.Equal(ts.MarketId, testTick.MarketId);
			Assert.Equal(ts.Value, testTick.Value);
			Assert.Equal(ts.StartDate, testTick.StartDate);
			Assert.Equal(ts.EndDate, testTick.EndDate);
			testTick.EndDate = Values.Mar1_2022;
			Assert.NotEqual(ts.EndDate, testTick.EndDate);
			Assert.NotEqual(ts, testTick);
		}
	}
}
