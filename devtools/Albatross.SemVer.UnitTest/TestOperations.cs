using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.SemVer.UnitTest {
	[TestFixture]
	public class TestOperations {

		[TestCase("2.1.1", ExpectedResult ="3.0.0")]
		[TestCase("1.0.0-a", ExpectedResult = "1.0.0")]
		[TestCase("0.0.0-a", ExpectedResult = "1.0.0")]
		[TestCase("0.1.1", ExpectedResult = "1.0.0")]
		[TestCase("0.0.0", ExpectedResult = "1.0.0")]
		[TestCase("1.0.0", ExpectedResult = "2.0.0")]
		public string TestNextMajor(string version) {
			return new SematicVersion(version).NextRelease(ReleaseType.Major).ToString();
		}

		[TestCase("2.1.1", ExpectedResult = "2.2.0")]
		[TestCase("1.0.0-a", ExpectedResult = "1.0.0")]
		[TestCase("0.0.0-a", ExpectedResult = "1.0.0")]
		[TestCase("0.1.1", ExpectedResult = "1.0.0")]
		[TestCase("0.0.0", ExpectedResult = "1.0.0")]
		[TestCase("1.0.0", ExpectedResult = "1.1.0")]
		public string TestNextMinor(string version) {
			return new SematicVersion(version).NextRelease(ReleaseType.Minor).ToString();
		}

		[TestCase("2.1.1", ExpectedResult = "2.1.2")]
		[TestCase("1.0.0-a", ExpectedResult = "1.0.0")]
		[TestCase("0.0.0-a", ExpectedResult = "1.0.0")]
		[TestCase("0.1.1", ExpectedResult = "1.0.0")]
		[TestCase("0.0.0", ExpectedResult = "1.0.0")]
		[TestCase("1.0.0", ExpectedResult = "1.0.1")]
		public string TestNextPatch(string version) {
			return new SematicVersion(version).NextRelease(ReleaseType.Patch).ToString();
		}

		[TestCase("1.0.0-b.0", "a", ExpectedResult = "1.0.0-b.1")]
		[TestCase("1.0.0-b", "a", ExpectedResult = "1.0.0-b.0")]

		[TestCase("1.0.0-a-9", "b", ExpectedResult = "1.0.0-b")]
		[TestCase("1.0.0-a", "b", ExpectedResult = "1.0.0-b")]
		[TestCase("1.0.0-a", "a", ExpectedResult = "1.0.0-a.0")]
		[TestCase("1.0.0-a.1", null, ExpectedResult = "1.0.0-a.2")]
		[TestCase("1.0.0-a.1", "a", ExpectedResult = "1.0.0-a.2")]

		[TestCase("1.0.0-1", "a", ExpectedResult = "1.0.0-a")]
		[TestCase("1.0.0-1", null, ExpectedResult = "1.0.0-2")]

		[TestCase("1.0.0", null, ExpectedResult = "1.0.1-0")]
		[TestCase("1.0.0", "a", ExpectedResult = "1.0.1-a")]
		[TestCase("0.0.0", "a", ExpectedResult = "0.0.1-a")]
		[TestCase("0.0.0", null, ExpectedResult = "0.0.1-0")]
		public string TestNextPrerelease(string version, string label) {
			return new SematicVersion(version).NextPrerelease(label).ToString();
		}
	}
}
