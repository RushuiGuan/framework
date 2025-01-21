using Albatross.Testing;
using FluentAssertions;
using Xunit;

namespace Sample.Hosting.Test {
	public class TestRandom {
		[Fact]
		public void TestRandomArray() {
			var array = new int[] { 
				1, 2, 3,4
			};
			array.Random().Should().BeOneOf(array);
		}
	}
}