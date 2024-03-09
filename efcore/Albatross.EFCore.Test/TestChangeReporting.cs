using Albatross.EFCore.ChangeReporting;
using Albatross.Hosting.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.EFCore;
using Sample.EFCore.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.EFCore.Test {
	public class TestChangeReporting {
		public class MyTestHost1 : MyTestHost {
			public string? ReportText { get; set; }
			public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
				base.RegisterServices(configuration, services);
				services.AddChangeReporting(new ChangeReportBuilder<MyData>()
					.Added()
					.IgnoreProperties(nameof(MyData.Property), nameof(MyData.ArrayProperty))
					.FixedHeaders(nameof(MyData.Id))
					.OnReportGenerated((text) => {
						this.ReportText = text;
						return Task.CompletedTask;
					}));
			}
		}
		[Fact]
		public async Task TestChangeReportingAdded() {
			var host = new MyTestHost1();
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			var set = session.DbContext.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.NotNull(host.ReportText);
		}
		
		public class MyTestHost2 : MyTestHost {
			public string? ReportText { get; set; }
			public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
				base.RegisterServices(configuration, services);
				services.AddChangeReporting(new ChangeReportBuilder<MyData>()
					.Modified()
					.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
					.FixedHeaders(nameof(MyData.Id))
					.Prefix("prefix\n")
					.Postfix("postfix")
					.OnReportGenerated((text) => {
						this.ReportText = text;
						return Task.CompletedTask;
					}));
			}
		}
		
		[Fact]
		public async Task TestChangeReportingModified() {
			var host = new MyTestHost2();
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			var set = session.DbContext.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.Null(host.ReportText);
			data.Bool = true;
			await session.SaveChangesAsync();
			Assert.NotNull(host.ReportText);
		}
		
		public class MyTestHost3 : MyTestHost {
			public string? ReportText { get; set; }
			public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
				base.RegisterServices(configuration, services);
				services.AddChangeReporting(new ChangeReportBuilder<MyData>()
					.Deleted()
					.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
					.FixedHeaders(nameof(MyData.Id))
					.Prefix("prefix\n")
					.Postfix("postfix")
					.OnReportGenerated((text) => {
						this.ReportText = text;
						return Task.CompletedTask;
					}));
			}
		}

		[Fact]
		public async Task TestChangeReportingDelete() {
			var host = new MyTestHost3();
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			var set = session.DbContext.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.Null(host.ReportText);
			set.Remove(data);
			await session.SaveChangesAsync();
			Assert.NotNull(host.ReportText);
		}

		public class MyTestHost4 : MyTestHost {
			public string? ReportText { get; set; }
			public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
				base.RegisterServices(configuration, services);
				services.AddChangeReporting(new ChangeReportBuilder<MyData>()
				.AllChangeTypes()
				.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
				.FixedHeaders(nameof(MyData.Id), nameof(MyData.Int))
				.Format(nameof(MyData.Int), "#,#0")
				.FormatFixedHeader(nameof(MyData.Int), "#,#0")
				.Prefix("prefix\n")
				.Postfix("postfix")
				.OnReportGenerated((text) => {
					this.ReportText = text;
					return Task.CompletedTask;
				}));
			}
		}

		[Fact]
		public async Task TestChangeReportingFormat() {
			var host = new MyTestHost4();
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			var set = session.DbContext.Set<MyData>();
			var data = new MyData {
				Int = 1000,
				DateTime = DateTime.UtcNow,
				UtcTimeStamp = DateTime.Now,
			};
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.NotNull(host.ReportText);
		}
	}
}