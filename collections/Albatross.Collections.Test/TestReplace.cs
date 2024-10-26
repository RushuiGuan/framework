using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Albatross.Collections.Test {
	public class TestReplace {
		public class A {
			public int Id { get; set; }
			public string Name { get; set; }
			public A(int id, string name) {
				this.Id = id;
				this.Name = name;
			}
		}

		[Fact]
		public void TestEmptyNewItems() {
			var existing = new A[]{
				new A(1, "a"),
				new A(2, "b"),
				new A(3, "c"),
			};
			var copy = existing.ToList();
			var newItems = new A[]{
			};
#pragma warning disable CS0618 // Type or member is obsolete
			existing.Replace(newItems, x => x.Id);
#pragma warning restore CS0618 // Type or member is obsolete
			existing.Should().BeEquivalentTo(copy);
		}

		[Fact]
		public void TestNoOverLap() {
			var existing = new List<A>{
				new A(1, "a"),
				new A(2, "b"),
				new A(3, "c"),
			};
			var copy = existing.ToList();
			var newItems = new A[]{
				new A(4, "d"),
				new A(5, "e"),
				new A(6, "f"),
			};
#pragma warning disable CS0618 // Type or member is obsolete
			existing.Replace(newItems, x => x.Id);
#pragma warning restore CS0618 // Type or member is obsolete
			existing.Should().BeEquivalentTo(copy.Union(newItems).ToArray());
		}

		[Fact]
		public void TestWithOverLap() {
			var existing = new List<A>{
				new A(1, "a"),
				new A(2, "b"),
				new A(3, "c"),
			};
			var copy = existing.ToList();
			var newItems = new A[]{
				new A(1, "d"),
				new A(5, "e"),
				new A(6, "f"),
			};
#pragma warning disable CS0618 // Type or member is obsolete
			existing.Replace(newItems, x => x.Id);
#pragma warning restore CS0618 // Type or member is obsolete
			existing.Should().BeEquivalentTo(copy.Where(x => x.Id != 1).Union(newItems));
		}
	}
}