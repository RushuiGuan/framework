using System.Collections.Generic;
using Xunit;
using System.Text.Json;
using System.Linq;

namespace Albatross.WebClient.Test {
	public partial class TestSerialization {
		public TestSerialization() {
		}

		[Fact]
		public void Test() {
			IEnumerable<string> result = JsonSerializer.Deserialize<IEnumerable<string>>("[\"test\"]");
			Assert.True(result?.Count() == 1);

			ISet<string> result2 = JsonSerializer.Deserialize<ISet<string>>("[\"test\", \"TEST\"]", new JsonSerializerOptions { 
			});
			Assert.True(result2?.Count() == 1);
		}
	}
}
