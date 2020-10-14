using Albatross.Hosting.Test;
using Albatross.Mapping.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using Xunit;

namespace Albatross.Mapping.UnitTest {
	public class TestRegex{
		[Theory]
		[InlineData("<base   href=\"\">")]
		[InlineData("< base href=\"abc\" >")]
		[InlineData("<base href=\"/a/b/c/\">")]
		public void TestPositive(string input) {
			//string pattern = "<\\s*base\\s+ href\\s*=\\s*\"[a-z0-9_-/]*\"\\s*>";
			string pattern = "<\\s*base\\s+href\\s*=\\s*\"[a-z0-9_\\-/]*\"\\s*>";
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
			var match = regex.Match(input);
			Assert.True(match.Success);
		}
	}
}
