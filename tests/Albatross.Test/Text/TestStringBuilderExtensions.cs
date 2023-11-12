using Albatross.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Test.Text {
	public class TestStringBuilderExtensions {
		[Theory]
		[InlineData("","", true)]
		[InlineData("a", "", true)]
		[InlineData("a", "a", true)]
		[InlineData("a", "b", false)]
		[InlineData("", "b", false)]
		[InlineData("b", "bb", false)]
		[InlineData("abc", "abc", true)]
		[InlineData("abc", "bc", true)]
		[InlineData("abc", "c", true)]
		[InlineData("abc", "b", false)]
		public void TestEndsWith(string textToSearch, string text, bool expectedResult) {
			var result = new StringBuilder(textToSearch).EndsWith(text);
			Assert.Equal(expectedResult, result);
		}
	}
}
