using Albatross.Testing;
using FluentAssertions;
using Xunit;

namespace Sample.Hosting.Test {
	public class TestIntArray {
		[Theory]
		[InlineData("1,2", "1,2")]
		[InlineData("1-3", "1,2,3")]
		[InlineData("1-e3", "2")]
		[InlineData("1-o3", "1,3")]
		public void TestString2IntArray(string text, string expected) {
			text.IntArray().AsString().Should().BeEquivalentTo(expected);
		}
	}
}