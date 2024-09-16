using Albatross.Hosting.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sample.EFCore;
using Sample.EFCore.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.EFCore.Test {
	public class TestSessionEventHandlerSetup {
		public class MyTestHost1 : MyTestHost {
			public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
				base.RegisterServices(configuration, services);
				services.TryAddEnumerable(ServiceDescriptor.Singleton<IDbSessionEventHandler, TestSessionEventHandler>());
			}
		}
		[Fact]
		public async Task TestAuditCreateChange() {
			var host = new MyTestHost1();
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			scope.Get<GetCurrentTestUser>().User = "xx";

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
			var host = new MyTestHost1();
			MyData data;
			DateTime audit;
			using (var scope = host.Create()) {
				var session = scope.Get<SampleDbSession>();
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

		public class MyTestHost2 : MyTestHost {
			public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
				base.RegisterServices(configuration, services);
				services.TryAddEnumerable(ServiceDescriptor.Scoped<IDbSessionEventHandler, ExceptionDbSessionEventHandler>());
			}
		}

		[Fact]
		public async Task TestSessionHandlerException() {
			var host = new MyTestHost2();
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
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