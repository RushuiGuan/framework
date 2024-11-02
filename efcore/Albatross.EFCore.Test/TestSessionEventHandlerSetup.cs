using Albatross.Testing.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sample.EFCore;
using Sample.EFCore.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.EFCore.Test {
	public class TestSessionEventHandlerSetup {
		void RegisterServices_1(IConfiguration configuration, IServiceCollection services) {
			My.RegisterServices(configuration, services);
			services.TryAddEnumerable(ServiceDescriptor.Singleton<IDbSessionEventHandler, TestSessionEventHandler>());
		}
		[Fact]
		public async Task TestAuditCreateChange() {
			using var host = new TestHostBuilder().RegisterServices(RegisterServices_1).Build();
			using var scope = host.Services.CreateScope();
			var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();
			scope.ServiceProvider.GetRequiredService<GetCurrentTestUser>().User = "xx";

			var set = session.DbContext.Set<MyData>();
			DateTime utcNow = DateTime.UtcNow;
			var data = new MyData();
			set.Add(data);

			await session.SaveChangesAsync();
			Assert.Equal("user0", data.CreatedBy);
			Assert.Equal("user0", data.ModifiedBy);
			Assert.True(data.CreatedUtc >= utcNow);
			Assert.True(data.ModifiedUtc >= utcNow);
		}

		[Fact]
		public async Task TestAuditUpdateChange() {
			using var host = new TestHostBuilder().RegisterServices(RegisterServices_1).Build();
			MyData data;
			DateTime audit;
			using (var scope = host.Services.CreateScope()) {
				var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();
				var set = session.DbContext.Set<MyData>();
				data = new MyData();
				set.Add(data);
				await session.SaveChangesAsync();
				data.Int = 100;
				audit = data.ModifiedUtc;
				await session.SaveChangesAsync();
			}

			Assert.True(data.CreatedUtc < data.ModifiedUtc);
			Assert.True(data.ModifiedUtc > audit);
			Assert.Equal("user0", data.CreatedBy);
			Assert.Equal("user1", data.ModifiedBy);
		}

		void RegisterServices_2(IConfiguration configuration, IServiceCollection services) {
			My.RegisterServices(configuration, services);
			services.TryAddEnumerable(ServiceDescriptor.Scoped<IDbSessionEventHandler, ExceptionDbSessionEventHandler>());
		}

		[Fact]
		public async Task TestSessionHandlerException() {
			using var host = new TestHostBuilder().RegisterServices(RegisterServices_2).Build();
			using var scope = host.Services.CreateScope();
			var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();
			var set = session.DbContext.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.NotEqual(0, data.Id);
			ExceptionDbSessionEventHandler.ThrowPriorSaveException = true;
			Assert.Throws<AggregateException>(() => session.SaveChanges());
			await Assert.ThrowsAsync<PreSaveException>(() => session.SaveChangesAsync());
		}
	}
}