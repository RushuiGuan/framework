using Albatross.Mapping.ByAutoMapper;
using Albatross.Hosting.Test;
using Albatross.Mapping.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using AutoMapper;

namespace Albatross.Mapping.UnitTest {
	public class TestAutoMapperRegistration : IClassFixture<TestAutoMapperRegistration.MyTestHost> {
		private readonly MyTestHost host;

		public class A {
			public string Name { get; set; }
		}
		public class AA {
			public string Name { get; set; }
		}

		public class ConfigMapping : IConfigMapping {
			public void Configure(IMapperConfigurationExpression expression) {
				expression.CreateMap<A, AA>().ReverseMap();
			}
		}

		public TestAutoMapperRegistration(MyTestHost host) {
			this.host = host;
		}

		public class MyTestHost : Albatross.Hosting.Test.TestHost {
			public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
				base.RegisterServices(configuration, services);
				services.AddMapping(this.GetType().Assembly);
				services.AddAutoMapperMapping().AddConfigMapping<ConfigMapping>().AddProfiles(this.GetType().Assembly);
			}
		}

		[Fact]
		public void TestMapping() {
			IMapper<A, AA> mapper = host.Provider.GetService<IMapper<A, AA>>();
			Assert.IsType<AutoMapperGeneric<A, AA>>(mapper);
			AA aa = new AA();
			A a = new A { Name = typeof(A).Name, };
			mapper.Map(a, aa);
			Assert.Equal(a.Name, aa.Name);
		}
	}
}
