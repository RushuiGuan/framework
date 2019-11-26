using Albatross.Host.Test;
using Albatross.Mapping.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Albatross.Mapping.UnitTest {
	public class TestRegistration : IClassFixture<TestRegistration.MyTestHost> {
		private readonly MyTestHost host;

		public class A{ }
		public class AA{ }
		public class AAMap : IMapper<A, AA> {
			public void Map(A src, AA dst) {
				throw new System.NotImplementedException();
			}
		}

		public TestRegistration(MyTestHost host) {
			this.host = host;
		}

		public class MyTestHost : Albatross.Host.Test.TestHost {
			public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
				base.RegisterServices(configuration, services);
				services.AddMapping(this.GetType().Assembly);
			}
		}
		[Fact]
		public void TestMapping() {
			IMapperFactory factory = host.Provider.GetService<IMapperFactory>();
			IMapper<A, AA> mapper = host.Provider.GetService<IMapper<A, AA>>();
			using (var scope = host.Create()){
				Assert.Same(factory, scope.Get<IMapperFactory>());
				Assert.Same(mapper, scope.Get<IMapper<A, AA>>());
			}
		}
	}
}
