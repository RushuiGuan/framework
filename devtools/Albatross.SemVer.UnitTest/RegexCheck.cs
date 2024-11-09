using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Albatross.SemVer.UnitTest {
	public class RegexCheck {
		[Theory]
		[InlineData("0", false)]
		[InlineData("000", true)]
		[InlineData("1", false)]
		[InlineData("10", false)]
		[InlineData("1000", false)]
		[InlineData("01", true)]
		[InlineData("001", true)]
		public void LeadingZero(string input, bool expected) {
			Assert.Equal(expected, SematicVersion.LeadingZeroNumericRegex.IsMatch(input));
		}


		[Theory]
		[InlineData("0", true)]
		[InlineData("000", false)]
		[InlineData("1", true)]
		[InlineData("10", true)]
		[InlineData("1000", true)]
		[InlineData("01", false)]
		[InlineData("001", false)]
		public void NonLeadingZero(string input, bool expected) {
			Assert.Equal(expected, SematicVersion.NonLeadingZeroNumericRegex.IsMatch(input));
		}

		public bool AlphaNumeric(string input) {
			return SematicVersion.AlphaNumericRegex.IsMatch(input);
		}

	}
}
