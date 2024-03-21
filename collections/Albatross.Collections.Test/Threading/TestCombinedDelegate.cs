using System;
using Xunit;

namespace Albatross.Test.Threading {
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
			try {
				action(data);
			} catch {
			}
			Assert.Equal(2, data.Value);
		}
	}
}
