using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Albatross.Test.RecordClass {
	public sealed record class MyDerivedRecord : MyRecord {
		public string Name { get; set; }
		public MyDerivedRecord(int id, string name) : base(id) {
			this.Name = name;
		}
	}
	public record class MyRecord {
		public int Id { get; set; }
		public MyRecord(int id) { Id = id; }
		public List<string> List { get; init; } = new List<string>();
		public Dictionary<string, string> Dict = new Dictionary<string, string>();
		public override int GetHashCode() {
			return new HashCode().From(Id)
				.FromCollection(List)
				.ToHashCode();
		}
		public virtual bool Equals(MyRecord? other) {
			if (other == null || other.GetType() != this.GetType()) return false;
			return Id == other.Id
				&& List.SequenceEqual(other.List)
				&& Dict.SequenceEqual(other.Dict);
		}
	}
	public class TestHashCode {
		[Fact]
		public void TestBasic() {
			var x = new MyRecord(1);
			x.List.Add("a");
			x.Dict["a"] = "b";
			var y = new MyRecord(1);
			y.List.Add("a");
			y.Dict["a"] = "b";
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
			var x = new MyDerivedRecord(1, "a");
			var y = new MyDerivedRecord(1, "a");
			Assert.Equal(x, y);
			Assert.Equal(x.GetHashCode(), y.GetHashCode());
			x.List.Add("b");
			Assert.NotEqual(x, y);
			Assert.NotEqual(x.GetHashCode(), y.GetHashCode());
			y.List.Add("b");
			Assert.Equal(x, y);
			Assert.Equal(x.GetHashCode(), y.GetHashCode());
			x.Name = "b";
			Assert.NotEqual(x, y);
			Assert.NotEqual(x.GetHashCode(), y.GetHashCode());
		}

		[Fact]
		public void TestDictEqual() {
			Dictionary<string, string> a = new Dictionary<string, string>();
			Dictionary<string, string> b = new Dictionary<string, string>();
			a["a"] = "b";
			b["a"] = "b";
			Assert.True(a.SequenceEqual(b));
			a["a"] = "c";
			Assert.False(a.SequenceEqual(b));
		}
		[Fact]
		public void TestKeyValuePairHashCode() {
			var a = new KeyValuePair<string, string>("a", "b");
			var b = new KeyValuePair<string, string>("a", "b");
			Assert.Equal(a, b);
			Assert.Equal(a.GetHashCode(), b.GetHashCode());
			var c = new KeyValuePair<string, string>("a", "c");
			Assert.NotEqual(a, c);
	//		Assert.NotEqual(a.GetHashCode(), c.GetHashCode());
			var d = new KeyValuePair<string, string>("b", "b");
			Assert.NotEqual(a, d);
			Assert.NotEqual(a.GetHashCode(), d.GetHashCode());
		}

		[Fact]
		public void TestRecordClassEqual() {
			var a = new MyDerivedRecord(1, "a");
			var b = new MyDerivedRecord(1, "a");
			Assert.True(a == b);
		}

		[Fact]
		public void TestRegexHashCode() {
			var a = new Regex("abc", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
			var b = new Regex("abc", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
			Assert.True(a.GetHashCode() == b.GetHashCode());
		}
	}
}