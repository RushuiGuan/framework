using FluentAssertions;
using System;
using System.Text.Json;
using Xunit;

namespace Albatross.Dates.Test {
	public class TestISO8601Conversion {
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
