using Albatross.EFCore.ChangeReporting;
using Albatross.Hosting.Test;
using Microsoft.EntityFrameworkCore;
using Sample.EFCore;
using Sample.EFCore.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.EFCore.Test {
	public class TestSessionEventHandlerSetup : IClassFixture<MyTestHost> {
		private readonly MyTestHost host;

		public TestSessionEventHandlerSetup(MyTestHost host) {
			this.host = host;
		}

		[Fact]
		public async Task TestAuditCreateChange() {
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			scope.Get<GetCurrentTestUser>().User = "xx";

			var set = session.DbContext.Set<MyData>();
			DateTime utcNow = DateTime.UtcNow;
			var data = new MyData();
			set.Add(data);

			await session.SaveChangesAsync();
			Assert.Equal("xx", data.CreatedBy);
			Assert.Equal("xx", data.ModifiedBy);
			Assert.True(data.CreatedUtc >= utcNow);
			Assert.True(data.ModifiedUtc >= utcNow);
		}

		[Fact]
		public async Task TestAuditUpdateChange() {
			MyData data;
			DateTime audit;
			using (var scope = host.Create()) {
				var session = scope.Get<SampleDbSession>();
				scope.Get<GetCurrentTestUser>().User = "create user";
				var set = session.DbContext.Set<MyData>();
				data = new MyData();
				set.Add(data);
				await session.SaveChangesAsync();
			}

			using (var scope = host.Create()) {
				scope.Get<GetCurrentTestUser>().User = "update user";
				var session = scope.Get<SampleDbSession>();
				var set = session.DbContext.Set<MyData>();
				data = await set.Where(x => x.Id == data.Id).FirstAsync();
				data.Int = 100;
				audit = data.ModifiedUtc;
				await session.SaveChangesAsync();
			}

			Assert.True(data.CreatedUtc < data.ModifiedUtc);
			Assert.True(data.ModifiedUtc > audit);
			Assert.Equal("create user", data.CreatedBy);
			Assert.Equal("update user", data.ModifiedBy);
		}

		[Fact]
		public async Task TestSessionHandlerException() {
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			var handler = new ExceptionDbSessionEventHandler(false);
			var set = session.DbContext.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.NotEqual(0, data.Id);

			handler.ThrowPriorSaveException = true;

			Assert.Throws<Exception>(() => session.SaveChanges());
			await Assert.ThrowsAsync<Exception>(() => session.SaveChangesAsync());
		}

		//[Fact]
		//public async Task TestChangeReportingAdded() {
		//	using var scope = host.Create();
		//	var session = scope.Get<SampleDbSession>();
		//	string? report = null;
		//	new ChangeReportBuilder<MyData>()
		//		.Added()
		//		.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
		//		.FixedHeaders(nameof(MyData.Id))
		//		.OnReportGenerated((text) => {
		//			report = text;
		//			return Task.CompletedTask;
		//		}).Build(session);
		//	var set = session.DbContext.Set<MyData>();
		//	var data = new MyData();
		//	set.Add(data);
		//	await session.SaveChangesAsync();
		//	Assert.NotNull(report);
		//}

		//[Fact]
		//public async Task TestChangeReportingModified() {
		//	using var scope = host.Create();
		//	var session = scope.Get<SampleDbSession>();
		//	string? report = null;
		//	new ChangeReportBuilder<MyData>()
		//		.Modified()
		//		.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
		//		.FixedHeaders(nameof(MyData.Id))
		//		.Prefix("prefix\n")
		//		.Postfix("postfix")
		//		.OnReportGenerated((text) => {
		//			report = text;
		//			return Task.CompletedTask;
		//		}).Build(session);
		//	var set = session.DbContext.Set<MyData>();
		//	var data = new MyData();
		//	set.Add(data);
		//	await session.SaveChangesAsync();
		//	Assert.Null(report);
		//	data.Bool = true;
		//	await session.SaveChangesAsync();
		//	Assert.NotNull(report);
		//}

		//[Fact]
		//public async Task TestChangeReportingDelete() {
		//	using var scope = host.Create();
		//	var session = scope.Get<SampleDbSession>();
		//	string? report = null;
		//	new ChangeReportBuilder<MyData>()
		//		.Deleted()
		//		.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
		//		.FixedHeaders(nameof(MyData.Id))
		//		.Prefix("prefix\n")
		//		.Postfix("postfix")
		//		.OnReportGenerated((text) => {
		//			report = text;
		//			return Task.CompletedTask;
		//		}).Build(session);
		//	var set = session.DbContext.Set<MyData>();
		//	var data = new MyData();
		//	set.Add(data);
		//	await session.SaveChangesAsync();
		//	Assert.Null(report);
		//	set.Remove(data);
		//	await session.SaveChangesAsync();
		//	Assert.NotNull(report);
		//}

		//[Fact]
		//public async Task TestChangeReportingFormat() {
		//	using var scope = host.Create();
		//	var session = scope.Get<SampleDbSession>();
		//	string? report = null;
		//	new ChangeReportBuilder<MyData>()
		//		.AllChangeTypes()
		//		.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
		//		.FixedHeaders(nameof(MyData.Id), nameof(MyData.Int))
		//		.Format(nameof(MyData.Int), "#,#0")
		//		.FormatFixedHeader(nameof(MyData.Int), "#,#0")
		//		.Prefix("prefix\n")
		//		.Postfix("postfix")
		//		.OnReportGenerated((text) => {
		//			report = text;
		//			return Task.CompletedTask;
		//		}).Build(session);
		//	var set = session.DbContext.Set<MyData>();
		//	var data = new MyData {
		//		Int = 1000,
		//		DateTime = DateTime.UtcNow,
		//		UtcTimeStamp = DateTime.Now,
		//	};
		//	set.Add(data);
		//	await session.SaveChangesAsync();
		//	Assert.NotNull(report);
		//}
	}
}