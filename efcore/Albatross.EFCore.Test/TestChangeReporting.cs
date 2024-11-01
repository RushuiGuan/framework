using Albatross.EFCore.ChangeReporting;
using Albatross.Testing.DependencyInjection;
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
		void RegisterServices_Report1(IConfiguration configuration, IServiceCollection services, Action<string> reportGenerated) {
			services.AddChangeReporting(new ChangeReportBuilder<MyData>()
				.Added()
				.IgnoreProperties(nameof(MyData.Property), nameof(MyData.ArrayProperty))
				.FixedHeaders(nameof(MyData.Id))
				.OnReportGenerated((text) => {
					reportGenerated(text);
					return Task.CompletedTask;
				}));
		}

		[Fact]
		public async Task TestChangeReportingAdded() {
			string? reportText = null;
			using var host = new TestHostBuilder().RegisterServices((cfg, services) => RegisterServices_Report1(cfg, services, x => reportText = x)).Build();
			using var scope = host.Services.CreateScope();
			var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();
			var set = session.DbContext.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.NotNull(reportText);
		}
		void RegisterServices_Report2(IConfiguration configuration, IServiceCollection services, Action<string> reportGenerated) {
			services.AddChangeReporting(new ChangeReportBuilder<MyData>()
					.Modified()
					.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
					.FixedHeaders(nameof(MyData.Id))
					.Prefix("prefix\n")
					.Postfix("postfix")
					.OnReportGenerated((text) => {
						reportGenerated(text);
						return Task.CompletedTask;
					}));
		}
		[Fact]
		public async Task TestChangeReportingModified() {
			string? reportText = null;
			using var host = new TestHostBuilder().RegisterServices((cfg, services) => RegisterServices_Report2(cfg, services, x => reportText = x)).Build();
			using var scope = host.Services.CreateScope();
			var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();
			var set = session.DbContext.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.Null(reportText);
			data.Bool = true;
			await session.SaveChangesAsync();
			Assert.NotNull(reportText);
		}
		void RegisterServices_Report3(IConfiguration configuration, IServiceCollection services, Action<string> reportGenerated) {
			services.AddChangeReporting(new ChangeReportBuilder<MyData>()
						.Deleted()
					.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
					.FixedHeaders(nameof(MyData.Id))
					.Prefix("prefix\n")
					.Postfix("postfix")
					.OnReportGenerated((text) => {
						reportGenerated(text);
						return Task.CompletedTask;
					}));
		}

		[Fact]
		public async Task TestChangeReportingDelete() {
			string? reportText = null;
			using var host = new TestHostBuilder().RegisterServices((cfg, services) => RegisterServices_Report3(cfg, services, x => reportText = x)).Build();
			using var scope = host.Services.CreateScope();
			var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();
			var set = session.DbContext.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.Null(reportText);
			set.Remove(data);
			await session.SaveChangesAsync();
			Assert.NotNull(reportText);
		}

		void RegisterServices_Report4(IConfiguration configuration, IServiceCollection services, Action<string> reportGenerated) {
			services.AddChangeReporting(new ChangeReportBuilder<MyData>()
				.AllChangeTypes()
				.IgnoreProperties(nameof(MyData.Id), nameof(MyData.Property), nameof(MyData.ArrayProperty))
				.FixedHeaders(nameof(MyData.Id), nameof(MyData.Int))
				.Format(nameof(MyData.Int), "#,#0")
				.FormatFixedHeader(nameof(MyData.Int), "#,#0")
				.Prefix("prefix\n")
				.Postfix("postfix")
				.OnReportGenerated((text) => {
					reportGenerated(text);
					return Task.CompletedTask;
				}));
		}
		[Fact]
		public async Task TestChangeReportingFormat() {
			string? reportText = null;
			using var host = new TestHostBuilder().RegisterServices((cfg, services) => RegisterServices_Report4(cfg, services, x => reportText = x)).Build();
			using var scope = host.Services.CreateScope();
			var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();
			var set = session.DbContext.Set<MyData>();
			var data = new MyData {
				Int = 1000,
				DateTime = DateTime.UtcNow,
				UtcTimeStamp = DateTime.Now,
			};
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.NotNull(reportText);
		}
	}
}