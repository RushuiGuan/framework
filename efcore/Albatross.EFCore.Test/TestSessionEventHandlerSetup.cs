using Albatross.EFCore.ChangeReporting;
using Albatross.Hosting.Test;
using Sample.EFCore;
using Sample.EFCore.Models;
using System;
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
			session.UseAuditEventHandlers<MyData>("test");
			var set = session.DbContext.Set<MyData>();
			DateTime utcNow = DateTime.UtcNow;
			var data = new MyData();
			set.Add(data);

			await session.SaveChangesAsync();
			Assert.Equal("test", data.CreatedBy);
			Assert.Equal("test", data.ModifiedBy);
			Assert.True(data.CreatedUtc >= utcNow);
			Assert.True(data.ModifiedUtc >= utcNow);
		}
		
		[Fact]
		public async Task TestAuditUpdateChange() {
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			session.UseAuditEventHandlers<MyData>("create user");
			var set = session.DbContext.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();

			session.SessionEventHandlers.Clear();
			session.UseAuditEventHandlers<MyData>("update user");
			var audit = data.ModifiedUtc;
			data.Int = 100;
			await session.SaveChangesAsync();
			
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
			session.SessionEventHandlers.Add(handler);
			var set = session.DbContext.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.NotEqual(0, data.Id);

			handler.ThrowPriorSaveException= true;

			Assert.Throws<Exception>(() => session.SaveChanges());
			await Assert.ThrowsAsync<Exception>(() => session.SaveChangesAsync());
		}

		[Fact]
		public async Task TestChangeReportingAdded() {
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			string? report = null;
			new ChangeReportBuilder<MyData>()
				.Added()
				.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
				.FixedHeaders(nameof(MyData.Id))
				.OnReportGenerated((text) => {
					report = text;
					return Task.CompletedTask;
				}).Build(session);
			var set = session.DbContext.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.NotNull(report);
		}

		[Fact]
		public async Task TestChangeReportingModified() {
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			string? report = null;
			new ChangeReportBuilder<MyData>()
				.Modified()
				.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
				.FixedHeaders(nameof(MyData.Id))
				.Prefix("prefix\n")
				.Postfix("postfix")
				.OnReportGenerated((text) => {
					report = text;
					return Task.CompletedTask;
				}).Build(session);
			var set = session.DbContext.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.Null(report);
			data.Bool = true;
			await session.SaveChangesAsync();
			Assert.NotNull(report);
		}

		[Fact]
		public async Task TestChangeReportingDelete() {
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			string? report = null;
			new ChangeReportBuilder<MyData>()
				.Deleted()
				.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
				.FixedHeaders(nameof(MyData.Id))
				.Prefix("prefix\n")
				.Postfix("postfix")
				.OnReportGenerated((text) => {
					report = text;
					return Task.CompletedTask;
				}).Build(session);
			var set = session.DbContext.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.Null(report);
			set.Remove(data);
			await session.SaveChangesAsync();
			Assert.NotNull(report);
		}

		[Fact]
		public async Task TestChangeReportingFormat() {
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			string? report = null;
			new ChangeReportBuilder<MyData>()
				.AllChangeTypes()
				.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
				.FixedHeaders(nameof(MyData.Id), nameof(MyData.Int))
				.Format(nameof(MyData.Int), "#,#0")
				.FormatFixedHeader(nameof(MyData.Int), "#,#0")
				.Prefix("prefix\n")
				.Postfix("postfix")
				.OnReportGenerated((text) => {
					report = text;
					return Task.CompletedTask;
				}).Build(session);
			var set = session.DbContext.Set<MyData>();
			var data = new MyData {
				Int = 1000,
				DateTime = DateTime.UtcNow,
				UtcTimeStamp = DateTime.Now,
			};
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.NotNull(report);
		}
	}
}