using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Albatross.Collections.Test {
	public class TestList {

		[Fact]
		public void IndexTest() {
			var list = new List<int>(100);
			for (int i = 0; i < 100; i++) {
				list.Add(0);
			}
			list[10] = 100;
			Assert.Equal(0, list[0]);
			Assert.Equal(100, list[10]);
		}

		[Fact]
		public void TestUnion() {
			int[] a = { 1, 2, 3 };
			int[] b = { 1, 2, 3 };
			var c = a.Union(b).ToArray();
			Assert.Equal(3, c.Length);
		}

		[Fact]
		public void TestUnion2() {
			byte[][] a = { new byte[0] };
			byte[][] b = { new byte[0] };
			var c = a.Union(b).ToArray();
			Assert.Equal(2, c.Length);
		}

		[Fact]
		public void TestUnion3() {
			string[] a = { "a" };
			string[] b = { "a", "b" };
			var c = a.Union(b).ToArray();
			Assert.Equal(2, c.Length);
		}

		[Fact]
		public void TestEncoding() {
			var text = Encoding.UTF8.GetString(new byte[0]);
			Assert.Equal(string.Empty, text);
		}

		[Fact]
		public void TestEncoding2() {
			var bytes = Encoding.UTF8.GetBytes(string.Empty);
			Assert.True(bytes.Length == 0);
		}
	}
}