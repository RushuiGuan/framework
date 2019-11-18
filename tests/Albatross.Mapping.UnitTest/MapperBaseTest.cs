using System;
using System.Collections.Generic;
using System.Text;
using Albatross.Mapping.Core;
using Xunit;

namespace Albatross.Mapping.UnitTest {
	
	public class MapperBaseTest {
		public class A {
			public string Property1 { get; set; }
		}
		public class B {
			public string Property1 { get; set; }
		}

		public class Mapper : MapperBase<A, B> {
			public override void Map(A src, B dst) {
				throw new NotImplementedException();
			}
		}

		[Fact]
		public void RunNormal() {
			var mapper = new Mapper();
			var a = new A { Property1 = "a" };
			var b = new B();
			mapper.Map(a, b);
			Assert.Equal(a.Property1, b.Property1);
		}
	}
}
