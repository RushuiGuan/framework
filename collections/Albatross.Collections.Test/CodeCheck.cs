using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Collections.Test {
	public class MyData {
		public string Name { get; set; }
		public MyData(string name, Action action) {
			Name = name;
			action();
		}
	}
	public class CodeCheck {
		IEnumerable<MyData> GetData(Action action) {
			for (int i = 0; i < 10; i++) {
				yield return new MyData((i % 2).ToString(), action);
			}
		}
		[Fact]
		public void TestLazy() {
			var count = 0;
			var data = GetData(() => count++);
			Assert.Equal(0, count);
			foreach (var item in data) {
				item.ToString();
			}
			Assert.Equal(10, count);
		}

		[Fact]
		public void TestLazyGrouping() {
			var count = 0;
			var data = GetData(() => count++).GroupBy(x => x.Name);
			Assert.Equal(0, count);
			foreach (var group in data) {
				foreach (var item in group) {
					item.ToString();
				}
			}
			Assert.Equal(10, count);
		}
	}
}
