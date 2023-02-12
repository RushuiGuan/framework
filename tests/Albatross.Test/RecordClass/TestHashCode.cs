using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Test.RecordClass {
	public sealed record class MyDerivedRecord : MyRecord {
		public string Name { get; set; }
		public MyDerivedRecord(int id) : base(id) {
		}
	}
	public record class MyRecord {
		public int Id { get; set; }
		public MyRecord(int id) { Id = id; }
		public List<string> List { get; init; } = new List<string>();
		public override int GetHashCode() {
			var hashCode = new HashCode();
			hashCode.Add(Id);
			List.ForEach(args => hashCode.Add(args));
			return hashCode.ToHashCode();
		}
		public virtual bool Equals(MyRecord? other) {
			if(other == null || other.GetType() != this.GetType()) return false;
			return Id == other.Id && List.SequenceEqual(other.List);
		}
	}
	public class TestHashCode {
		[Fact]
		public void TestBasic() {
			var x = new MyRecord(1);
			x.List.Add("a");
			var y = new MyRecord(1);
			y.List.Add("a");
			Assert.Equal(x.GetHashCode(), y.GetHashCode());

			var x_hash = new HashCode();
			x_hash.Add(x);
			var x_hashcode = x_hash.ToHashCode();
			var y_hash = new HashCode();	
			y_hash.Add(y);
			var y_hashcode = y_hash.ToHashCode();
			Assert.Equal(x_hashcode, y_hashcode);

			Assert.NotEqual(x.GetHashCode(), x_hashcode);

			x.List.Add("b1");
			y.List.Add("b2");
			Assert.NotEqual(x.GetHashCode(), y.GetHashCode());
		}
		[Fact]
		public void TestDerived() {
			var x = new MyDerivedRecord(1);
			var y = new MyDerivedRecord(1);
			Assert.Equal(x, y);
			Assert.Equal(x.GetHashCode(), y.GetHashCode());
			x.List.Add("b");
			Assert.NotEqual(x.GetHashCode(), y.GetHashCode());
		}
	}
}
