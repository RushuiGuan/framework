using Albatross.Reqnroll;
using FluentAssertions;
using Reqnroll;
using System;
using System.Collections.Generic;
using Xunit;

namespace Albatross.Testing.UnitTest {
	public record class TestClass {
		public DateOnly Date { get; set; }
		public string? Name { get; set; }
	}
	public class TestDataTableConversion {
		[Fact]
		public void Run() {
			var items = new List<TestClass> {
				new TestClass { Date = new DateOnly(2021, 1, 1), Name = "Alice" },
				new TestClass { Date = new DateOnly(2021, 1, 2), Name = "Bob" },
			};
			var table = items.DataTable();

			var expected = new DataTable("Date", "Name");
			expected.AddRow("2021-01-01", "Alice");
			expected.AddRow("2021-01-02", "Bob");
			expected.Should().BeEquivalentTo(table);
		}
	}
}
