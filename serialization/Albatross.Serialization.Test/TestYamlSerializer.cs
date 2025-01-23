using System;
using System.Text.Json;
using Xunit;

namespace Albatross.Serialization.Test {
	public class TestYamlSerializer {

		[Theory]
		[InlineData("2021-01-01", "2021-01-01T00:00:00")]
		[InlineData("2021-01-01Z", "2021-01-01T00:00:00Z")]
		public void TestDateTimeSerialization(string value, string expected) {
			var date = DateTime.Parse(value, null, System.Globalization.DateTimeStyles.RoundtripKind);
			Assert.Equal(expected, Yaml.Extensions.DefaultYamlSerializer().Serialize(date).Trim());
		}

		[Theory]
		[InlineData("2021-01-01", "2021-01-01")]
		public void TestDateOnlySerialization(string value, string expected) {
			var date = DateOnly.Parse(value);
			Assert.Equal(expected, Yaml.Extensions.DefaultYamlSerializer().Serialize(date).Trim());
		}

		[Theory]
		[InlineData("1", "1")]
		[InlineData("\"a\"", "'a'")]
		public void TestJsonElementSerialization(string text, string expected) {
			var value = JsonDocument.Parse(text).RootElement;
			var result = Yaml.Extensions.DefaultYamlSerializer().Serialize(value).Trim();
			Assert.Equal(expected, result);
		}
	}
}
