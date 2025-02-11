using Albatross.Text.Table;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Albatross.Text.Test {
	public class TestTableBuilder {
		public class TestClass {
			public int Id { get; set; }
			public string? Name { get; set; }
			public double Value { get; set; }
		}
		[Fact]
		public void TestDefaultFormat() {
			object? obj = null;
			BuilderExtensions.DefaultFormat(obj).Should().Be("");
			obj = 1;
			BuilderExtensions.DefaultFormat(obj).Should().Be("1");
			obj = 1.0M;
			BuilderExtensions.DefaultFormat(obj).Should().Be("1");
			obj = "1";
			BuilderExtensions.DefaultFormat(obj).Should().Be("1");
			obj = 1.0;
			BuilderExtensions.DefaultFormat(obj).Should().Be("1");
			obj = new DateOnly(2000, 1, 1);
			BuilderExtensions.DefaultFormat(obj).Should().Be("2000-01-01");
			obj = new DateTime(2000, 1, 1, 1, 1, 1);
			BuilderExtensions.DefaultFormat(obj).Should().Be("2000-01-01T01:01:01");
			obj = DateTime.SpecifyKind(new DateTime(2000, 1, 1, 1, 1, 1), DateTimeKind.Utc);
			BuilderExtensions.DefaultFormat(obj).Should().Be("2000-01-01T01:01:01Z");
			obj = new DateTimeOffset(new DateTime(2000, 1, 1, 1, 1, 1), TimeSpan.FromHours(1));
			BuilderExtensions.DefaultFormat(obj).Should().Be("2000-01-01T01:01:01+01:00");
		}

		[Fact]
		public void TestColumnOrders() {
			var builder = new TableOptionBuilder<TestClass>().SetColumnsByReflection();
			// test default order
			var options = builder.Build();
			options.ColumnOptions.Select(x => new { x.Property, x.Order }).Should().BeEquivalentTo(new[] {
				new { Property = nameof(TestClass.Id), Order = 0 },
				new { Property = nameof(TestClass.Name), Order = 1 },
				new { Property = nameof(TestClass.Value), Order = 2 },
			});
			// test order by the Order property
			builder.ColumnOrder(x => x.Value, -1);
			options = new TableOptions<TestClass>(builder);
			options.ColumnOptions.Select(x => new { x.Property, x.Order }).Should().BeEquivalentTo(new[] {
				new { Property = nameof(TestClass.Value), Order = -1 },
				new { Property = nameof(TestClass.Id), Order = 0 },
				new { Property = nameof(TestClass.Name), Order = 1 },
			});
			// test order by header
			builder.ColumnOrder(x => x.Id, 0);
			builder.ColumnOrder(x => x.Name, 0);
			builder.ColumnOrder(x => x.Value, 0);
			builder.ColumnHeader(x => x.Id, "C");
			builder.ColumnHeader(x => x.Name, "A");
			builder.ColumnHeader(x => x.Value, "B");
			options = new TableOptions<TestClass>(builder);
			options.ColumnOptions.Select(x => new { x.Property, x.Order, x.Header }).Should().BeEquivalentTo(new[] {
				new { Property = nameof(TestClass.Name), Order = 0, Header = "A" },
				new { Property = nameof(TestClass.Value), Order = 0, Header = "B" },
				new { Property = nameof(TestClass.Id), Order = 0, Header = "C" },
			});
			// test order by property
			builder.ColumnOrder(x => x.Id, 0);
			builder.ColumnOrder(x => x.Name, 0);
			builder.ColumnOrder(x => x.Value, 0);
			builder.ColumnHeader(x => x.Id, "C");
			builder.ColumnHeader(x => x.Name, "C");
			builder.ColumnHeader(x => x.Value, "C");
			options = new TableOptions<TestClass>(builder);
			options.ColumnOptions.Select(x => new { x.Property, x.Order, x.Header }).Should().BeEquivalentTo(new[] {
				new { Property = nameof(TestClass.Id), Order = 0, Header = "C" },
				new { Property = nameof(TestClass.Name), Order = 0, Header = "C" },
				new { Property = nameof(TestClass.Value), Order = 0, Header = "C" },
			});
		}

		[Fact]
		public void TestHeaders() {
			var builder = new TableOptionBuilder<TestClass>().SetColumnsByReflection();
			builder.ColumnHeader(x => x.Id, "C");
			builder.ColumnHeader(x => x.Name, "A");
			builder.ColumnHeader(x => x.Value, "B");
			var options = new TableOptions<TestClass>(builder);
			options.ColumnOptions.Select(x => new { x.Property, x.Header }).Should().BeEquivalentTo(new[] {
				new { Property = nameof(TestClass.Id), Header = "C" },
				new { Property = nameof(TestClass.Name), Header = "A" },
				new { Property = nameof(TestClass.Value), Header = "B" },
			});
		}

		[Fact]
		public void TestGetValueDelegate() {
			var builder = new TableOptionBuilder<TestClass>().SetColumnsByReflection();
			var options = new TableOptions<TestClass>(builder);
			var obj = new TestClass { Id = 1, Name = "name", Value = 1.0 };
			var values = options.GetValue(obj);
			values.Should().BeEquivalentTo(new[] { "1", "name", "1" });
		}

		[Fact]
		public void TestFormatter() {
			var builder = new TableOptionBuilder<TestClass>().SetColumnsByReflection();
			builder.Format(x => x.Value, "0.00");
			var options = new TableOptions<TestClass>(builder);
			var obj = new TestClass { Id = 1, Name = "name", Value = 1.0 };
			var values = options.GetValue(obj);
			values.Should().BeEquivalentTo(new[] { "1", "name", "1.00" });
		}

		[Fact]
		public void TestBuilderFactory() {
			var options = new TableOptionBuilder<TestClass>().SetColumnsByReflection();
			Assert.NotNull(options);
			TableOptionFactory.Instance.Register(new TableOptionBuilder<TestClass>());
			var options2 = TableOptionFactory.Instance.Get<TestClass>();
			Assert.NotSame(options, options2);
		}
	}
}