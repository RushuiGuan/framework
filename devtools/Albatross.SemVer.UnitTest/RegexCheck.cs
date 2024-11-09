using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.SemVer.UnitTest
{
	[TestFixture]
    public class RegexCheck
    {
		[TestCase("0", ExpectedResult =false)]
		[TestCase("000", ExpectedResult = true)]
		[TestCase("1", ExpectedResult = false)]
		[TestCase("10", ExpectedResult = false)]
		[TestCase("1000", ExpectedResult = false)]
		[TestCase("01", ExpectedResult = true)]
		[TestCase("001", ExpectedResult = true)]
		public bool LeadingZero(string input) {
			return SematicVersion.LeadingZeroNumericRegex.IsMatch(input);
		}


		[TestCase("0", ExpectedResult = true)]
		[TestCase("000", ExpectedResult = false)]
		[TestCase("1", ExpectedResult = true)]
		[TestCase("10", ExpectedResult = true)]
		[TestCase("1000", ExpectedResult = true)]
		[TestCase("01", ExpectedResult = false)]
		[TestCase("001", ExpectedResult = false)]
		public bool NonLeadingZero(string input) {
			return SematicVersion.NonLeadingZeroNumericRegex.IsMatch(input);
		}

		public bool AlphaNumeric(string input) {
			return SematicVersion.AlphaNumericRegex.IsMatch(input);
		}

	}
}
