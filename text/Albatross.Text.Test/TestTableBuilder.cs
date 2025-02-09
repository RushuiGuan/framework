using Albatross.Text.Table;
using Xunit;

namespace Albatross.Text.Test {
	public class TestTableBuilder {
		public class TestClass {
			public int Id { get; set; }
			public string? Name { get; set; }
			public double Value { get; set; }
		}

		[Fact]
		public void TestColumnOrders() {
			var builder = new TableBuilder<TestClass>();
			var options = new TableOptions<TestClass>(builder);
			Assert.Equal(nameof(TestClass.Id), options.ColumnHeaders[0]);
			Assert.Equal(nameof(TestClass.Name), options.ColumnHeaders[1]);
			Assert.Equal(nameof(TestClass.Value), options.ColumnHeaders[2]);

			builder.ColumnOrder(nameof(TestClass.Name), -1);
			options = new TableOptions<TestClass>(builder);
			Assert.Equal(nameof(TestClass.Name), options.ColumnHeaders[0]);
			Assert.Equal(nameof(TestClass.Id), options.ColumnHeaders[1]);
			Assert.Equal(nameof(TestClass.Value), options.ColumnHeaders[2]);
		}
	}
}