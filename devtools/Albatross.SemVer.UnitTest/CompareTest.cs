using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using Xunit;

namespace Albatross.SemVer.UnitTest {
	public class CompareTest {

		[Theory]
		[InlineData("1.2.3", "1.2.3",  0)]
		[InlineData("1.2.3+1", "1.2.3+2",  0)]
		[InlineData("0.2.3", "1.2.3",  -1)]
		[InlineData("1.2.3", "1.3.3",  -1)]
		[InlineData("1.2.3", "1.2.4",  -1)]
		[InlineData("1.2.3-alpha", "1.2.3",  -1)]
		[InlineData("1.2.3-alpha", "1.2.3-alpha",  0)]
		[InlineData("1.2.3-alpha", "1.2.3-alpha.0",  -1)]
		public void Compare(string a, string b, int expectedResult) {
			var sem1 = new SematicVersion(a);
			var sem2 = new SematicVersion(b);
			int result_a = sem1.CompareTo(sem2);
			int result_b = sem2.CompareTo(sem1);


			if (result_a == 0) {
				Assert.Equal(0, result_b);
			} else {
				Assert.Equal(result_a * -1, result_b);
			}

			Assert.Equal(expectedResult, result_a);
		}


		/// <summary>
		/// reference behavior of compare function
		/// </summary>
		[InlineData("a", "b", -1)]
		[InlineData("a", "a", 0)]
		[InlineData("b", "a", 1)]
		[Theory]
		public void CompareString(string a, string b, int expectedResult) {
			Assert.Equal(expectedResult, a.CompareTo(b));
		}
	}
}
