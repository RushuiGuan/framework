using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Text.Test {
	public class TestGlobPatternMatching {


		[Theory]
		[InlineData("test", "*", true)]
		[InlineData("test", "t*", true)]
		[InlineData("test", "*t", true)]
		[InlineData("test", "t???", true)]
		[InlineData("test", "t??", false)]
		[InlineData("test", "a*", false)]
		[InlineData("", "*", false)]
		[InlineData(null, "*", false)]
		public void Test(string input, string pattern, bool expected) {
			var result = input.Like(pattern);
			Assert.Equal(expected, result);
		}
	}

}
