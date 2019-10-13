using Albatross.Host.NUnit;
using Albatross.Mapping.Core;
using Autofac;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Albatross.Mapping.UnitTest {
	[TestFixture]
	public class Test : TestBase<TestUnitOfWork> {

		public override void RegisterPackages(IServiceCollection svc) {
			svc.AddMapping();
			svc.AddSingleton<IConfigMapping, CfgMapping>();
		}

		public class CfgMapping : IConfigMapping {
			public void Configure(IMapperConfigurationExpression expression) {
				expression.CreateMap<From, To>();
				expression.CreateMap<From, From>();
				expression.CreateMap<To, To>();
			}
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

		[Test]
		public void UseAutoMapper() {
			From a = new From {
				A1 = 100,
				B1 = 101,
			};

            using (var unitOfWork= NewUnitOfWork())
            {
                IMapper mapper = unitOfWork.Get<IMapper>();
                Assert.NotNull(mapper);
                To b = mapper.Map<From, To>(a);
                Assert.AreEqual(a.A1, b.A1);
                Assert.AreEqual(a.B1, b.B1);
            }
		}

		[Test]
		public void UseGenericMapper() {
			From a = new From {
				A1 = 100,
				B1 = 101,
			};
            using (var unitOfWork = NewUnitOfWork())
            {
				var factory = unitOfWork.Get<IMapperFactory>();
				var mapper = factory.Get<From, To>();

                Assert.NotNull(mapper);
                To b = mapper.Map(a);
                Assert.AreEqual(a.A1, b.A1);
                Assert.AreEqual(a.B1, b.B1);
            }
		}

		[Test]
		public void UseCustomMapper() {
			var svc = new ServiceCollection();
			svc.AddMapping();
			svc.AddSingleton<IConfigMapping, CfgMapping>();
			svc.AddSingleton<IMapper<From, To>, CustomMap>();
			var provider = svc.BuildServiceProvider();

			From a = new From();

			var mapper = provider.GetRequiredService<IMapper<From, To>>();
			Assert.NotNull(mapper);
			To b = mapper.Map(a);
			Assert.AreEqual(999, b.A1);
			Assert.AreEqual(888, b.B1);
		}
	}
}
