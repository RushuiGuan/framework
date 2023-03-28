using Albatross.Hosting.Test;
using Albatross.Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Repository.Test {
	public class TestResetDateLevelEntity {
		static int Compare(TickSize x, TickSize y) => x.StartDate.CompareTo(y.StartDate);

		[Fact]
		public async Task ResetTimeSeries1() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevel<TickSize, int>(new TickSize(1, Values.Apr1_2022, 500), list.Add, args => list.Remove(args), true);

			list.Sort(Compare);

			Assert.Collection(list,
				args => {
					Assert.Equal(Values.Mar1_2022, args.StartDate);
					Assert.Equal(Values.Mar31_2022, args.EndDate);
					Assert.Equal(100, args.Value);
				},
				args => {
					Assert.Equal(Values.Apr1_2022, args.StartDate);
					Assert.Equal(Values.MaxSqlDate, args.EndDate);
					Assert.Equal(500, args.Value);
				}
			);
		}

		[Fact]
		public async Task ResetTimeSeries2() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await set.Object.SetDateLevel<TickSize, int>(new TickSize(1, Values.Mar1_2022, 100), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevel<TickSize, int>(new TickSize(1, Values.Jul1_2022, 200), list.Add, args => list.Remove(args));
			await set.Object.SetDateLevel<TickSize, int>(new TickSize(1, Values.Sep1_2022, 300), list.Add, args => list.Remove(args), true);

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
	}
}
