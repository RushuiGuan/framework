using Albatross.Host.Test;
using Albatross.Mapping.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Albatross.Mapping.UnitTest {
	public class TestMapping : IClassFixture<MappingTestHost> {
		private readonly MappingTestHost host;

		public TestMapping(MappingTestHost host) {
			this.host = host;
		}

		public class CustomMap : IMapper<From, To> {
			public void Map(From src, To dst) {
				dst.A1 = 999;
				dst.B1 = 888;
			}
		}


		public class From {
			public int A1 { get; set; }
			public int B1 { get; set; }
		}

		public class To {
			public int A1 { get; set; }
			public int B1 { get; set; }
		}

		[Fact]
		public void UseGenericMapper() {
			From a = new From {
				A1 = 100,
				B1 = 101,
			};
			using (var scope = host.Create()) {
				var factory = scope.Get<IMapperFactory>();
				var mapper = factory.Get<From, To>();

				Assert.NotNull(mapper);
				To b = mapper.Map(a);
				Assert.Equal(a.A1, b.A1);
				Assert.Equal(a.B1, b.B1);
			}
		}

		[Fact]
		public void UseCustomMapper() {
			var svc = new ServiceCollection();
			svc.AddMapping();
			svc.AddSingleton<IMapper<From, To>, CustomMap>();
			var provider = svc.BuildServiceProvider();

			From a = new From();

			var mapper = provider.GetRequiredService<IMapper<From, To>>();
			Assert.NotNull(mapper);
			To b = mapper.Map(a);
			Assert.Equal(999, b.A1);
			Assert.Equal(888, b.B1);
		}
	}
}
