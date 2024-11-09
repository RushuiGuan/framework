using System;
using Xunit;

namespace Albatross.SemVer.UnitTest {
	public class ParsingTest
    {
		[Theory]
		[InlineData("0.0.0-0+0")]
		//a standard version
		[InlineData("1.2.3-label.4.5+meta.6.7")]
		[InlineData("1.2.3-LABEL.4.5+META.6.7")]		//upper case check
		//version only
		[InlineData("1.2.3")]
		//version and pre-release only
		[InlineData("1.2.3-beta")]
		[InlineData("1.2.3-beta.2")]

		//version and metadata only
		[InlineData("1.2.3+meta.6.7")]
		[InlineData("1.2.3+meta")]
		[InlineData("1.2.3+meta67")]
		[InlineData("1.2.3+000")] //metadata can have leading zeros

		[InlineData("1.2.3-x+y-1-2-3")] //try to confuse the parser with hyphens

		[InlineData("1.2.3-rc.999")]
		[InlineData("1.2.3-a.b")]
		[InlineData("1.2.3-a.1.2")]
		[InlineData("1.2.3-1.1.2")]
		[InlineData("1.2.3-2-abc")]
		[InlineData("1.2.3-2")]
		[InlineData("1.2.3-beta.0")]
		[InlineData("0.0.0")]
		[InlineData("0.1.2-3+4")]
		[InlineData("0.0.0-xx.0+yy.0")]
		public void ValidVersionString(string input) {
			var semver = new SematicVersion(input);
			Assert.Equal(input, semver.ToString());
		}

		[Theory]
		//some edge cases
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		[InlineData("+-")]

		//leading zero @ version
		[InlineData("01.2.3-label.4.5+meta.6.7")]
		[InlineData("1.02.3-label.4.5+meta.6.7")]
		[InlineData("1.2.03-label.4.5+meta.6.7")]

		//leading zero @ prerelease
		[InlineData("1.2.3-label.04.5+meta.6.7")]
		//invalid char in prelease
		[InlineData("1.2.3-label_4.5+meta.6.7")]
		//invalid char in metadata
		[InlineData("1.2.3-label.4.5+meta_6.7")]

		//version problem
		[InlineData("1.1.1.1")]
		[InlineData(".1.1.1")]
		[InlineData("1.1.1.")]
		[InlineData("1. 1.1")]
		[InlineData("1.1.1 ")]
		[InlineData(" 1.1.1")]
		[InlineData("a.1.1")]
		[InlineData("1.b.1")]
		[InlineData("1.1.c")]
		[InlineData("-1.1.1")]
		[InlineData("1-1-1")]
		[InlineData("...")]
		[InlineData("1")]
		[InlineData("1.2")]

		//metadata and prerelease char is reversed
		[InlineData("1.1.1+a-b")]
		//empty metadata
		[InlineData("1.2.3+ ")]
		//empty prerelease
		[InlineData("1.2.3- ")]
		public void InvalidVersionString(string input) {
			Assert.ThrowsAny<Exception>(() => new SematicVersion(input));
		}
	}
}
