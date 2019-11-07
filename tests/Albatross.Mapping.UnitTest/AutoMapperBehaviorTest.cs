using AutoMapper;
using Xunit;

namespace Albatross.Mapping.UnitTest {
	public class AutoMapperBehaviorTest{
		public class A {
			public int Number1 { get; set; }
			public int Number2 { get; set; }
		}

		public class AA {
			public int Number1 { get; set; }
		}

		[Fact]
		public void Run() {
			var config = new MapperConfiguration(cfg => cfg.CreateMap<A, AA>().ReverseMap());
			var mapper = config.CreateMapper();
			mapper.Map(new AA(), new A());
			mapper.Map(new A(), new AA());
		}
	}
}
