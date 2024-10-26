using System;
using Xunit;

namespace Albatross.Thrading.Test {
	public class MyData {
		public int Value { get; set; }
	}
	public class TestCombinedDelegate {
		[Fact]
		public void CheckCreation() {
			Action<MyData> action = x => x.Value++;
			action += x => throw new Exception("x");
			action += x => x.Value++;
			var data = new MyData();
			Assert.ThrowsAny<Exception>(() => action(data));
			Assert.Equal(1, data.Value);
		}
	}
}