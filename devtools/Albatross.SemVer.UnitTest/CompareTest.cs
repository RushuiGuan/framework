using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Albatross.SemVer.UnitTest
{
	[TestFixture]
    public class CompareTest
    {

		[TestCase("1.2.3","1.2.3", ExpectedResult =0)]
		[TestCase("1.2.3+1", "1.2.3+2", ExpectedResult = 0)]
		[TestCase("0.2.3", "1.2.3", ExpectedResult = -1)]
		[TestCase("1.2.3", "1.3.3", ExpectedResult = -1)]
		[TestCase("1.2.3", "1.2.4", ExpectedResult = -1)]
		[TestCase("1.2.3-alpha", "1.2.3", ExpectedResult = -1)]
		[TestCase("1.2.3-alpha", "1.2.3-alpha", ExpectedResult = 0)]
		[TestCase("1.2.3-alpha", "1.2.3-alpha.0", ExpectedResult = -1)]
		public int Compare(string a, string b) {
			var sem1 = new SematicVersion(a);
			var sem2 = new SematicVersion(b);
			int result_a = sem1.CompareTo(sem2);
			int result_b = sem2.CompareTo(sem1);


			if (result_a == 0) {
				Assert.Zero(result_b);
			} else {
				Assert.AreEqual(result_a * -1, result_b);
			}

			return result_a;
		}


		/// <summary>
		/// reference behavior of compare function
		/// </summary>
		[TestCase("a", "b", ExpectedResult = -1)]
		[TestCase("a", "a", ExpectedResult = 0)]
		[TestCase("b", "a", ExpectedResult = 1)]
		public int CompareString(string a, string b) {
			return a.CompareTo(b);
		}
	}
}
