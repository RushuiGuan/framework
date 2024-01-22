using Albatross.Hosting.Test;
using Sample.EFCore.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Albatross.DateLevel.Test {
	public class TestSetDateTimeLevelEntity {
		[Fact]
		public void Baseline() {
			var list = new List<ContractSpec>();
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		/// set 1
		[Fact]
		public void Mar100_Jul200_Sep300_Nov300() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Nov1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Sep300() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Aug300() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Aug1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jul31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Aug1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jul300() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jun300() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jun1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.May31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jun1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Mar300() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Feb300() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Feb1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Feb28_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		/// set 2
		[Fact]
		public void Mar100_Jul200_Sep300_Nov400() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Nov1_2022, 400), true);
			list.Sort(Compare);;

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Oct31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Nov1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(400, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Sep400() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(400, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Aug400() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Aug1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jul31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Aug1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jul400() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jun400() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jun1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.May31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jun1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Mar400() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Feb400() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Feb1_2022, 400), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Feb28_2022, args.EndDate);
					Assert.Equal(400, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}
		static int Compare(ContractSpec x, ContractSpec y) => x.StartDate.CompareTo(y.StartDate);
		/// set 3
		[Fact]
		public void Mar100_Jul200_Sep300_Jun100() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jun1_2022, 100), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Mar100() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Feb100() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Feb1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Feb28_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		///set 4
		[Fact]
		public void Mar100_Jul200_Sep300_Nov200() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Nov1_2022, 200), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Oct31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Nov1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Sep200() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 200), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Aug200() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Aug1_2022, 200), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jul200() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Jun200() {
			var list = new List<ContractSpec>();


			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jun1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.May31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jun1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Mar200() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
					args => {
						Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
						Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
						Assert.Equal(300, args.Value);
					},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Sep300_Feb200() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Feb1_2022, 300), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Feb28_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Jul200() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(200, args.Value);
				}
			);
		}

		[Fact]
		public void Mar100_Jul200_Feb150_Insert() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Feb1_2022, 150) { EndDate = DateTimeValues.Mar31_2022}, true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Feb1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Mar31_2022, args.EndDate);
					Assert.Equal(150, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Apr1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jun30_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Jul1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate); 
					Assert.Equal(200, args.Value);
				}
			);
		}
		[Fact]
		public void Mar100_Jul200_Sep300_Apr150_Insert() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Apr1_2022, 150) { EndDate = DateTimeValues.Jul31_2022 }, true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Mar31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Apr1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Jul31_2022, args.EndDate);
					Assert.Equal(150, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Aug1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Aug31_2022, args.EndDate);
					Assert.Equal(200, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Sep1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(300, args.Value);
				}
			);
		}
		[Fact]
		public void Mar100_Jul200_Sep300_Nov400_Apr150_Insert() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Nov1_2022, 400), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Apr1_2022, 150) { EndDate = DateTimeValues.Sep30_2022 }, true);
			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(DateTimeValues.Mar1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Mar31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Apr1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Sep30_2022, args.EndDate);
					Assert.Equal(150, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Oct1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.Oct31_2022, args.EndDate);
					Assert.Equal(300, args.Value);
				},
				args => {
					Assert.Equal(DateTimeValues.Nov1_2022, args.StartDate);
					Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
					Assert.Equal(400, args.Value);
				}
			);
		}
		[Fact]
		public void Mar100_Jul200_Sep300_Insert() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			Assert.Throws<InvalidOperationException>(() =>
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Sep1_2022, 300) {
				EndDate = DateTimeValues.Nov30_2022,
			}, true));
		}
		[Fact]
		public void Mar100_Jul200_Jun300_Insert() {
			var list = new List<ContractSpec>();

			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Mar1_2022, 100), true);
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jul1_2022, 200), true);
			Assert.Throws<InvalidOperationException>(() =>
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jun1_2022, 300) {
				EndDate = DateTimeValues.Jun30_2022,
			}, true));
		}

		[Fact]
		public void SameValue_Insert() {
			var list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Jan1_2022, 100) {
					EndDate = DateTimeValues.Mar31_2022
				},
				new ContractSpec(1, DateTimeValues.Apr1_2022, 100) {
					EndDate = DateTimeValues.Jun30_2022,
				},
				new ContractSpec(1, DateTimeValues.Jul1_2022, 100) {
					EndDate = DateTimeValues.Oct31_2022,
				},
				new ContractSpec(1, DateTimeValues.Nov1_2022, 100) {
					EndDate = DateTimeValues.MaxSqlDate
				},
			};
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.May1_2022, 100), true);
			list.Sort(Compare);
			Assert.Collection(list, args => { 
				Assert.Equal(DateTimeValues.Jan1_2022, args.StartDate);
				Assert.Equal(DateTimeValues.Mar31_2022, args.EndDate);
			},
			args => { 
				Assert.Equal(DateTimeValues.Apr1_2022, args.StartDate);
				Assert.Equal(DateTimeValues.Oct31_2022, args.EndDate);
			},
			args => {
				Assert.Equal(DateTimeValues.Nov1_2022, args.StartDate);
				Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
			});
		}
		[Fact]
		public void SameValue_Insert2() {
			var list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Jan1_2022, 100) {
					EndDate = DateTimeValues.Mar31_2022
				},
				new ContractSpec(1, DateTimeValues.Apr1_2022, 100) {
					EndDate = DateTimeValues.Jun30_2022,
				},
				new ContractSpec(1, DateTimeValues.Jul1_2022, 100) {
					EndDate = DateTimeValues.Oct31_2022,
				},
				new ContractSpec(1, DateTimeValues.Nov1_2022, 100) {
					EndDate = DateTimeValues.MaxSqlDate
				},
			};
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.May1_2022, 100) {
				EndDate = DateTimeValues.Aug1_2022
			}, true);
			list.Sort(Compare);
			Assert.Collection(list, args => {
				Assert.Equal(DateTimeValues.Jan1_2022, args.StartDate);
				Assert.Equal(DateTimeValues.Mar31_2022, args.EndDate);
			},
			args => {
				Assert.Equal(DateTimeValues.Apr1_2022, args.StartDate);
				Assert.Equal(DateTimeValues.Oct31_2022, args.EndDate);
			},
			args => {
				Assert.Equal(DateTimeValues.Nov1_2022, args.StartDate);
				Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
			});
		}
		[Fact]
		public void SameValue_Append() {
			var list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Jan1_2022, 100) {
					EndDate = DateTimeValues.Mar31_2022
				},
				new ContractSpec(1, DateTimeValues.Apr1_2022, 100) {
					EndDate = DateTimeValues.Jun30_2022,
				},
				new ContractSpec(1, DateTimeValues.Jul1_2022, 100) {
					EndDate = DateTimeValues.Oct31_2022,
				},
				new ContractSpec(1, DateTimeValues.Nov1_2022, 100) {
					EndDate = DateTimeValues.MaxSqlDate
				},
			};
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.May1_2022, 100), false);
			list.Sort(Compare);
			Assert.Collection(list, args => {
				Assert.Equal(DateTimeValues.Jan1_2022, args.StartDate);
				Assert.Equal(DateTimeValues.Mar31_2022, args.EndDate);
			},
			args => {
				Assert.Equal(DateTimeValues.Apr1_2022, args.StartDate);
				Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
			});
		}

		[Fact]
		public void SingleValue_Insert() {
			var list = new List<ContractSpec> {
				new ContractSpec(1, DateTimeValues.Feb1_2022, 1) {
					EndDate = DateTimeValues.Feb1_2022
				},
				new ContractSpec(1, DateTimeValues.Feb2_2022, 2) {
					EndDate = DateTimeValues.MaxSqlDate
				},
			};
			list.SetDateLevel<ContractSpec, int>(new ContractSpec(1, DateTimeValues.Jan1_2022, 3), true);
			list.Sort(Compare);
			Assert.Collection(list, args => {
				Assert.Equal(3, args.Value);
				Assert.Equal(DateTimeValues.Jan1_2022, args.StartDate);
				Assert.Equal(DateTimeValues.Jan31_2022, args.EndDate);
			},
			args => {
				Assert.Equal(1, args.Value);
				Assert.Equal(DateTimeValues.Feb1_2022, args.StartDate);
				Assert.Equal(DateTimeValues.Feb1_2022, args.EndDate);
			},
			args => {
				Assert.Equal(2, args.Value);
				Assert.Equal(DateTimeValues.Feb2_2022, args.StartDate);
				Assert.Equal(DateTimeValues.MaxSqlDate, args.EndDate);
			});
		}
	}
}
