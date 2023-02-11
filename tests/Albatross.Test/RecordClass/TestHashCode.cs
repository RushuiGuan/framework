using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Test.RecordClass {
	public record class MyRecord {
		public string Name { get; set; }
		public MyRecord(string name) { Name = name; }
	}
	public class TestHashCode {
		[Fact]
		public void TestBasic() {
			var x = new MyRecord("a");
			var y = new MyRecord("a");
			Assert.Equal(x.GetHashCode(), y.GetHashCode());
			var x_hash = new HashCode();
			x_hash.Add(x);
			var x_hashcode = x_hash.ToHashCode();
			var y_hash = new HashCode();	
			y_hash.Add(y);
			var y_hashcode = y_hash.ToHashCode();
			Assert.Equal(x_hashcode, y_hashcode);

			Assert.NotEqual(x.GetHashCode(), x_hashcode);
		}

		[Fact]
		public void TestArray() {
			var x = new MyRecord[0];
			var y = new MyRecord[0];
			Assert.Equal(x.GetHashCode(), y.GetHashCode());
			var x_hash = new HashCode();
			x_hash.Add(x);
			var x_hashcode = x_hash.ToHashCode();
			var y_hash = new HashCode();
			y_hash.Add(y);
			var y_hashcode = y_hash.ToHashCode();
			Assert.Equal(x_hashcode, y_hashcode);
		}
	}
}
