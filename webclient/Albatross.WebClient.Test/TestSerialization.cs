using System.Collections.Generic;
using Xunit;
using System.Text.Json;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Moq;
using Albatross.Serialization;
using System;
using FluentAssertions;
using Albatross.Dates;

namespace Albatross.WebClient.Test {
	public class TestClient : ClientBase {
		public TestClient(ILogger logger, HttpClient client, IJsonSettings serializationOption) : base(logger, client, serializationOption) {
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
			var client = new TestClient(new Mock<ILogger>().Object, new HttpClient(), new DefaultJsonSettings());
			Assert.Throws<JsonException>(() => {
				var result = client.Deserialize<ServiceError>(text);
			});
		}

		[Theory]
		[InlineData("{\"xx\" : 1}")]
		[InlineData("{\"message\" : \"dd\" }")]
		[InlineData("{\"message\" : \"dd\", \"my\": \"yes\" }")]
		public void TestDeserialization(string text) {
			var client = new TestClient(new Mock<ILogger>().Object, new HttpClient(), new DefaultJsonSettings());
			var result = client.Deserialize<ServiceError>(text);
			Assert.NotNull(result);
		}

		[Fact]
		public void TestDateTimeSerialization() {
			var local = new DateTime(2024, 9, 12, 14, 30, 44, DateTimeKind.Local);
			var utc = DateTime.SpecifyKind(local, DateTimeKind.Utc);
			var unspecified = DateTime.SpecifyKind(local, DateTimeKind.Unspecified);

			var localText_expected = JsonSerializer.Serialize(local);
			var utcText_expected = JsonSerializer.Serialize(utc);
			var unspecifiedText_expected = JsonSerializer.Serialize(unspecified);

			local.ISO8601String().Should().Be(localText_expected.Trim('"'));
			utc.ISO8601String().Should().Be(utcText_expected.Trim('"'));
			unspecified.ISO8601String().Should().Be(unspecifiedText_expected.Trim('"'));
		}

		[Fact]
		public void TestDateTimeOffsetSerialization() {
			var value = new DateTimeOffset(2024, 9, 12, 14, 30, 44, TimeSpan.FromHours(3));
			var value_expected = JsonSerializer.Serialize(value);
			value.ISO8601String().Should().Be(value_expected.Trim('"'));
		}

		[Fact]
		public void TestDateOnly() {
			var value = new DateOnly(2024, 9, 12);
			var value_expected = JsonSerializer.Serialize(value);
			$"{value:yyyy-MM-dd}".Should().Be(value_expected.Trim('"'));
		}

		[Fact]
		public void TestTimeOnly() {
			var value = new TimeOnly(14, 20, 40, 123);
			var value_expected = JsonSerializer.Serialize(value);
			$"{value:HH:mm:ss.fffffff}".Should().Be(value_expected.Trim('"'));
		}
	}
}
