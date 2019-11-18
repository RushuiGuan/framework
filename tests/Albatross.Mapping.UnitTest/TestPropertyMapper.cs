using Albatross.Mapping.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Albatross.Mapping.UnitTest {

	public class TestPropertyMapper : IClassFixture<MappingTestHost> {
		private readonly MappingTestHost host;

		public TestPropertyMapper(MappingTestHost host) {
			this.host = host;
		}

		public class Operation {
			public string Name { get; set; }
			public User CreatedBy { get; set; }
		}
		public class OperationDto {
			public string FullName { get; set; }
			public string CreatedBy { get; set; }
		}
		public class User {
			public string Name { get; set; }
		}
		public class UserDto {
			public string Name { get; set; }
		}


		[Fact]
		public void BasicTest() {
			using (var scope = host.Create()) {
				var factory = scope.Get<IMapperFactory>();
				PropertyMapper<Operation, OperationDto> m = new PropertyMapper<Operation, OperationDto>(factory);
				m.For(args => args.FullName).Use(args => args.Name);
				m.For(args => args.CreatedBy).Use(args => args.CreatedBy);
			}
		}
	}
}
