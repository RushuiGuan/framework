using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Albatross.SemVer.UnitTest {
	public class TestOperations {

		[Theory]
		[InlineData("2.1.1", "3.0.0")]
		[InlineData("1.0.0-a", "1.0.0")]
		[InlineData("0.0.0-a", "1.0.0")]
		[InlineData("0.1.1", "1.0.0")]
		[InlineData("0.0.0", "1.0.0")]
		[InlineData("1.0.0", "2.0.0")]
		public void TestNextMajor(string version, string expected) {
			Assert.Equal(expected, new SematicVersion(version).NextRelease(ReleaseType.Major).ToString());
		}

		[Theory]
		[InlineData("2.1.1", "2.2.0")]
		[InlineData("1.0.0-a", "1.0.0")]
		[InlineData("0.0.0-a", "1.0.0")]
		[InlineData("0.1.1", "1.0.0")]
		[InlineData("0.0.0", "1.0.0")]
		[InlineData("1.0.0", "1.1.0")]
		public void TestNextMinor(string version, string expected) {
			Assert.Equal(expected, new SematicVersion(version).NextRelease(ReleaseType.Minor).ToString());
		}

		[Theory]
		[InlineData("2.1.1", "2.1.2")]
		[InlineData("1.0.0-a", "1.0.0")]
		[InlineData("0.0.0-a", "1.0.0")]
		[InlineData("0.1.1", "1.0.0")]
		[InlineData("0.0.0", "1.0.0")]
		[InlineData("1.0.0", "1.0.1")]
		public void TestNextPatch(string version, string expected) {
			Assert.Equal(expected, new SematicVersion(version).NextRelease(ReleaseType.Patch).ToString());
		}

		[Theory]
		[InlineData("1.0.0-b.0", "a", "1.0.0-b.1")]
		[InlineData("1.0.0-b", "a", "1.0.0-b.0")]

		[InlineData("1.0.0-a-9", "b", "1.0.0-b")]
		[InlineData("1.0.0-a", "b", "1.0.0-b")]
		[InlineData("1.0.0-a", "a", "1.0.0-a.0")]
		[InlineData("1.0.0-a.1", null, "1.0.0-a.2")]
		[InlineData("1.0.0-a.1", "a", "1.0.0-a.2")]

		[InlineData("1.0.0-1", "a", "1.0.0-a")]
		[InlineData("1.0.0-1", null, "1.0.0-2")]

		[InlineData("1.0.0", null, "1.0.1-0")]
		[InlineData("1.0.0", "a", "1.0.1-a")]
		[InlineData("0.0.0", "a", "0.0.1-a")]
		[InlineData("0.0.0", null, "0.0.1-0")]
		public void TestNextPrerelease(string version, string? label, string expected) {
			Assert.Equal(expected, new SematicVersion(version).NextPrerelease(label).ToString());
		}
	}
}
