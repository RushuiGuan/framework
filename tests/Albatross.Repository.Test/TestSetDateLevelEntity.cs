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
		public async Task FirstRow() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await new TickSize(1, Values.Jan1_2022, Values.Jan1_2022, 10, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);
			Assert.Collection(list, args => {
				Assert.Equal(Values.Jan1_2022, args.StartDate);
				Assert.Equal(Values.MaxSqlDate, args.EndDate);
				Assert.Equal(Values.Any, args.CreatedBy);
				Assert.Equal(Values.Any, args.ModifiedBy);
				Assert.Equal(10, args.Value);
			});
		}

		[Fact]
		public async Task Prior_Date_Diff_Value_Exists() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await new TickSize(1, Values.Jan1_2022, null, 10, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			await new TickSize(1, Values.Feb1_2022, null, 20, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);
			Assert.Collection(list, args => {
				Assert.Equal(Values.Jan1_2022, args.StartDate);
				Assert.Equal(Values.Jan31_2022, args.EndDate);
				Assert.Equal(10, args.Value);
			},
			args => {
				Assert.Equal(Values.Feb1_2022, args.StartDate);
				Assert.Equal(Values.MaxSqlDate, args.EndDate);
				Assert.Equal(20, args.Value);
			});
		}

		[Fact]
		public async Task Prior_Date_Same_Value_Exists() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await new TickSize(1, Values.Jan1_2022, null, 10, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			await new TickSize(1, Values.Feb1_2022, null, 10, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);
			Assert.Collection(list, args => {
				Assert.Equal(Values.Jan1_2022, args.StartDate);
				Assert.Equal(Values.MaxSqlDate, args.EndDate);
				Assert.Equal(10, args.Value);
			});
		}

		[Fact]
		public async Task Same_Date_Diff_Value_Exists() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await new TickSize(1, Values.Feb1_2022, null, 10, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			await new TickSize(1, Values.Feb1_2022, null, 20, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);
			Assert.Collection(list, args => {
				Assert.Equal(Values.Feb1_2022, args.StartDate);
				Assert.Equal(Values.MaxSqlDate, args.EndDate);
				Assert.Equal(20, args.Value);
			});
		}

		[Fact]
		public async Task Same_Date_Same_Value_Exists() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await new TickSize(1, Values.Feb1_2022, null, 10, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			await new TickSize(1, Values.Feb1_2022, null, 10, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);
			Assert.Collection(list, args => {
				Assert.Equal(Values.Feb1_2022, args.StartDate);
				Assert.Equal(Values.MaxSqlDate, args.EndDate);
				Assert.Equal(10, args.Value);
			});
		}

		[Fact]
		public async Task Next_Date_Diff_Value_Exists() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await new TickSize(1, Values.Feb1_2022, null, 10, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			await new TickSize(1, Values.Jan1_2022, null, 20, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);
			Assert.Collection(list, args => {
				Assert.Equal(Values.Feb1_2022, args.StartDate);
				Assert.Equal(Values.MaxSqlDate, args.EndDate);
				Assert.Equal(10, args.Value);
			},
			args => {
				Assert.Equal(Values.Jan1_2022, args.StartDate);
				Assert.Equal(Values.Jan31_2022, args.EndDate);
				Assert.Equal(20, args.Value);
			});
		}

		[Fact]
		public async Task Next_Date_Same_Value_Exists() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await new TickSize(1, Values.Feb1_2022, null, 10, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			await new TickSize(1, Values.Jan1_2022, null, 10, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);
			Assert.Collection(list, args => {
				Assert.Equal(Values.Jan1_2022, args.StartDate);
				Assert.Equal(Values.MaxSqlDate, args.EndDate);
				Assert.Equal(10, args.Value);
			});
		}


		[Fact]
		public async Task Prior_Next_Date_Diff_Value_Exists() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await new TickSize(1, Values.Feb1_2022, null, 1, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			await new TickSize(1, Values.Sep1_2022, null, 2, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			await new TickSize(1, Values.Apr1_2022, null, 3, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			Assert.Collection(list, args => {
				Assert.Equal(Values.Feb1_2022, args.StartDate);
				Assert.Equal(Values.Mar31_2022, args.EndDate);
				Assert.Equal(1, args.Value);
			},
			args => {
				Assert.Equal(Values.Sep1_2022, args.StartDate);
				Assert.Equal(Values.MaxSqlDate, args.EndDate);
				Assert.Equal(2, args.Value);
			},
			args => {
				Assert.Equal(Values.Apr1_2022, args.StartDate);
				Assert.Equal(Values.Aug31_2022, args.EndDate);
				Assert.Equal(3, args.Value);
			});
		}

		[Fact]
		public async Task Prior_Next_Date_Same_Value_Exists() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await new TickSize(1, Values.Feb1_2022, null, 1, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			await new TickSize(1, Values.Sep1_2022, null, 1, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			await new TickSize(1, Values.Apr1_2022, null, 1, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			Assert.Collection(list, args => {
				Assert.Equal(Values.Feb1_2022, args.StartDate);
				Assert.Equal(Values.MaxSqlDate, args.EndDate);
				Assert.Equal(1, args.Value);
			});
		}

		[Fact]
		public async Task Set_DateLevel_Value_And_RemovePostDateEntries() {
			List<TickSize> list = new List<TickSize>();
			var set = list.CreateAsyncDbSet<TickSize>();
			await new TickSize(1, Values.Feb1_2022, null, 1, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			await new TickSize(1, Values.Sep1_2022, null, 2, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any);

			await new TickSize(1, Values.Jan1_2022, null, 3, Values.Any)
				.SetDateLevel(set.Object, args => list.Add(args), args => list.Remove(args), Values.Any, true);

			Assert.Collection(list, args => {
				Assert.Equal(Values.Jan1_2022, args.StartDate);
				Assert.Equal(Values.MaxSqlDate, args.EndDate);
				Assert.Equal(3, args.Value);
			});
		}
	}
}
