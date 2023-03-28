using Albatross.Hosting.Test;
using Albatross.Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Repository.Test {
	public class TestSetDateLevelEntity {
		[Fact]
		public async Task Baseline() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Nov300() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Nov1_2022, 300), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Sep300() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Aug300() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Aug1_2022, 300), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Jul300() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 300), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Jun300() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jun1_2022, 300), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Mar300() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 300), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Feb300() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Feb1_2022, 300), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Nov400() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Nov1_2022, 400), list.Add, args => list.Remove(args));
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
					Assert.Equal(400, args.Value);
				}
			);
		}

		[Fact]
		public async Task Mar100_Jul200_Sep300_Sep400() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 400), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Aug400() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Aug1_2022, 400), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Jul400() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 400), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Jun400() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jun1_2022, 400), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Mar400() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 400), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Feb400() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Feb1_2022, 400), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Jun100() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jun1_2022, 100), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Mar100() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 300), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Feb100() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Feb1_2022, 300), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Nov200() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Nov1_2022, 200), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Sep200() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 200), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Aug200() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Aug1_2022, 200), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Jul200() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 300), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Jun200() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();

			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jun1_2022, 300), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Mar200() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 300), list.Add, args => list.Remove(args));
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
		public async Task Mar100_Jul200_Sep300_Feb200() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevelAsync<TickSize, int>(new TickSize(1, Values.Feb1_2022, 300), list.Add, args => list.Remove(args));
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
	}
}
