using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			var result = instance.Interpolate<IDictionary<string, string>>(input, (key,values) => values[key], values);
			Assert.Equal(expectedResult, result);
		}
	}
}
