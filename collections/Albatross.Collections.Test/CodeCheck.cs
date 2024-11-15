using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Collections.Test {
	public class MyData {
		public static int Count = 0;
		public string Name { get; set; }
		public MyData(string name) {
			Name = name;
			Count++;
		}
	}
	public class CodeCheck {
		IEnumerable<MyData> GetData() {
			for (int i = 0; i < 10; i++) {
				yield return new MyData((i % 2).ToString());
			}
		}
		[Fact]
		public void TestLazy() {
			var data = GetData();
			Assert.Equal(0, MyData.Count);
			foreach (var item in data) {
				item.ToString();
			}
			Assert.Equal(10, MyData.Count);
		}

		[Fact]
		public void TestLazyGrouping() {
			var data = GetData().GroupBy(x => x.Name);
			Assert.Equal(0, MyData.Count);
			foreach (var group in data) {
				foreach (var item in group) {
					item.ToString();
				}
			}
			Assert.Equal(10, MyData.Count);
		}
	}
}
