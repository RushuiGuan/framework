using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.SemVer.UnitTest
{
	[TestFixture]
    public class ParsingTest
    {
		[TestCase("0.0.0-0+0")]
		//a standard version
		[TestCase("1.2.3-label.4.5+meta.6.7")]
		[TestCase("1.2.3-LABEL.4.5+META.6.7")]		//upper case check
		//version only
		[TestCase("1.2.3")]
		//version and pre-release only
		[TestCase("1.2.3-beta")]
		[TestCase("1.2.3-beta.2")]

		//version and metadata only
		[TestCase("1.2.3+meta.6.7")]
		[TestCase("1.2.3+meta")]
		[TestCase("1.2.3+meta67")]
		[TestCase("1.2.3+000")] //metadata can have leading zeros

		[TestCase("1.2.3-x+y-1-2-3")] //try to confuse the parser with hyphens

		[TestCase("1.2.3-rc.999")]
		[TestCase("1.2.3-a.b")]
		[TestCase("1.2.3-a.1.2")]
		[TestCase("1.2.3-1.1.2")]
		[TestCase("1.2.3-2-abc")]
		[TestCase("1.2.3-2")]
		[TestCase("1.2.3-beta.0")]
		[TestCase("0.0.0")]
		[TestCase("0.1.2-3+4")]
		[TestCase("0.0.0-xx.0+yy.0")]
		public void ValidVersionString(string input) {
			var semver = new SematicVersion(input);
			Assert.AreEqual(input, semver.ToString());
		}

		//some edge cases
		[TestCase(null)]
		[TestCase("")]
		[TestCase("   ")]
		[TestCase("+-")]

		//leading zero @ version
		[TestCase("01.2.3-label.4.5+meta.6.7")]
		[TestCase("1.02.3-label.4.5+meta.6.7")]
		[TestCase("1.2.03-label.4.5+meta.6.7")]

		//leading zero @ prerelease
		[TestCase("1.2.3-label.04.5+meta.6.7")]
		//invalid char in prelease
		[TestCase("1.2.3-label_4.5+meta.6.7")]
		//invalid char in metadata
		[TestCase("1.2.3-label.4.5+meta_6.7")]

		//version problem
		[TestCase("1.1.1.1")]
		[TestCase(".1.1.1")]
		[TestCase("1.1.1.")]
		[TestCase("1. 1.1")]
		[TestCase("1.1.1 ")]
		[TestCase(" 1.1.1")]
		[TestCase("a.1.1")]
		[TestCase("1.b.1")]
		[TestCase("1.1.c")]
		[TestCase("-1.1.1")]
		[TestCase("1-1-1")]
		[TestCase("...")]
		[TestCase("1")]
		[TestCase("1.2")]

		//metadata and prerelease char is reversed
		[TestCase("1.1.1+a-b")]
		//empty metadata
		[TestCase("1.2.3+ ")]
		//empty prerelease
		[TestCase("1.2.3- ")]

		public void InvalidVersionString(string input) {
			TestDelegate testDelegate = new TestDelegate(() => {
				var semver = new SematicVersion(input);
			});
			Assert.Catch(testDelegate);
		}
	}
}
