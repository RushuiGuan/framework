using Albatross.Hosting.Test;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Albatross.Collections.Test {
	public class TestExtensions {

		[Theory]
		[InlineData("4-6", 5, true, "4,6")]
		[InlineData("1-o10", 4, false, "1,3,5,7,9")]
		[InlineData("1,1,2,3", 1, true, "1,2,3")]
		public void TryGetOneAndRemove(string array, int target, bool expected, string expected_text) {
			var list = array.IntArray().ToList();
			var old_length = list.Count;
			var has = list.TryGetOneAndRemove(x=>x == target, out var result);
			has.Should().Be(expected);
			list.AsString().Should().Be(expected_text);
			if (expected) {
				result.Should().Be(target);
			}
		}
	}
}
