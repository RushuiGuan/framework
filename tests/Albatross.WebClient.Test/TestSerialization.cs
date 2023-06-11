using System.Collections.Generic;
using Xunit;
using System.Text.Json;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Moq;
using Albatross.Serialization;

namespace Albatross.WebClient.Test {
	public class TestClient : ClientBase {
		public TestClient(ILogger logger, HttpClient client, IJsonSerializationOption serializationOption) : base(logger, client, serializationOption) {
		}
	}
	public partial class TestSerialization {
		public TestSerialization() {
		}

		[Fact]
		public void Test() {
			IEnumerable<string> result = JsonSerializer.Deserialize<IEnumerable<string>>("[\"test\"]");
			Assert.True(result?.Count() == 1);

			ISet<string> result2 = JsonSerializer.Deserialize<ISet<string>>("[\"test\", \"TEST\"]", new JsonSerializerOptions {
			});
			Assert.True(result2?.Count() == 2);
		}

		[Theory]
		[InlineData("")]
		[InlineData("{\"message\" : 1}")]
		public void TestDeserializationError(string text) {
			var client = new TestClient(new Mock<ILogger>().Object, new HttpClient(), new DefaultJsonSerializationOption());
			Assert.Throws<JsonException>(() => {
				var result = client.Deserialize<ServiceError>(text);
			});
		}

		[Theory]
		[InlineData("{\"xx\" : 1}")]
		[InlineData("{\"message\" : \"dd\" }")]
		[InlineData("{\"message\" : \"dd\", \"my\": \"yes\" }")]
		public void TestDeserialization(string text) {
			var client = new TestClient(new Mock<ILogger>().Object, new HttpClient(), new DefaultJsonSerializationOption());
			var result = client.Deserialize<ServiceError>(text);
			Assert.NotNull(result);
		}
	}
}
