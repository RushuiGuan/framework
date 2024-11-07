using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace Albatross.Text.Test {
	public class TestTemplateStringInterpolation {

		public ILogger<StringInterpolationService> Logger() => new Moq.Mock<ILogger<StringInterpolationService>>().Object;

		IDictionary<string, string> values = new Dictionary<string, string> {
			{ "b", "1"},
			{ "", "x"},
		};


		[Theory]
		[InlineData("test", "test")]
		[InlineData("${a}", "${a}")]
		[InlineData("${}", "${}")]
		[InlineData("${b}", "1")]
		[InlineData("${b}abc", "1abc")]
		[InlineData("abc${b}", "abc1")]
		[InlineData("abc${b}abc", "abc1abc")]
		[InlineData("${b}${b}${b}", "111")]
		public void Test(string input, string expectedResult) {
			var instance = new StringInterpolationService(Logger());
			var result = input.Interpolate<IDictionary<string, string>>((key, values) => values[key], values);
			Assert.Equal(expectedResult, result);
		}


		[Theory]
		[InlineData("abc_${b}_abc", @"{""b"":""1""}", "abc_1_abc")]
		[InlineData("abc_${b}_abc_${b}", @"{""b"":""1""}", "abc_1_abc_1")]
		public void TestStringInterpolationWithoutObject(string text, string dictionary, string expected) {
			var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(dictionary) ?? new Dictionary<string, string>();
			var result = text.Interpolate(args => dict[args]);
			Assert.Equal(expected, result);
		}
	}
}