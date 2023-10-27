using Albatross.Reflection;
using ExcelDna.Integration;
using System;
using System.Text.Json;
using Xunit;

namespace Albatross.Excel.Test {
	public class TestCellValueUtility {

		[Theory]
		[InlineData(1, true, 1)]
		[InlineData("1", true, 1)]
		[InlineData(1.1, true, 1)]
		[InlineData(1.5, true, 2)]
		[InlineData("a", false, 0)]
		[InlineData(ExcelError.ExcelErrorGettingData, false, 0)]
		public void TestTryReadInt(object value,  bool shouldRead, int expectedResult) {
			var hasValue = CellValue.TryReadInteger(value, out var result);
			Assert.Equal(shouldRead, hasValue);
			if (hasValue) {
				Assert.Equal(expectedResult, result);
			}
		}

		[Theory]
		[InlineData(1, true, 1)]
		[InlineData("1", true, 1)]
		[InlineData(1.1, true, 1.1)]
		[InlineData(1.5, true, 1.5)]
		[InlineData("a", false, 0)]
		[InlineData(ExcelError.ExcelErrorGettingData, false, 0)]
		public void TestTryReadDouble(object value, bool shouldRead, double expectedResult) {
			var hasValue = CellValue.TryReadDouble(value, out var result);
			Assert.Equal(shouldRead, hasValue);
			if (hasValue) {
				Assert.Equal(expectedResult, result);
			}
		}

		[Theory]
		[InlineData(ExcelError.ExcelErrorNull, false, false, null)]
		[InlineData("", false, false, null)]
		[InlineData(45226, false, true, "2023-10-27")]
		[InlineData("2023-01-01", false, true, "2023-01-01")]
		[InlineData("2023-01-01", true, true, "2023-01-01")]
		public void TestTryReadDateOnly(object value, bool parse, bool shouldRead, string expectedResult) {
			if(parse) {
				value = DateTime.Parse((string)value);
			}
			var hasValue = CellValue.TryReadDateOnly(value, out var result);
			Assert.Equal(shouldRead, hasValue);
			if (hasValue) {
				Assert.Equal(DateOnly.Parse(expectedResult), result);
			}
		}

		[Theory]
		[InlineData(ExcelError.ExcelErrorNull, false, false, null)]
		[InlineData("", false, false, null)]
		[InlineData(45226, false, true, "2023-10-27")]
		[InlineData("2023-01-01", false, true, "2023-01-01")]
		[InlineData("2023-01-01", true, true, "2023-01-01")]
		public void TestTryReadDateTime(object value, bool parse, bool shouldRead, string expectedResult) {
			if (parse) {
				value = DateTime.Parse((string)value);
			}
			var hasValue = CellValue.TryReadDateTime(value, out var result);
			Assert.Equal(shouldRead, hasValue);
			if (hasValue) {
				Assert.Equal(DateTime.Parse(expectedResult), result);
			}
		}

		[Theory]
		[InlineData(ExcelError.ExcelErrorNull, false, false)]
		[InlineData("", false, false)]

		[InlineData(1,  true, true)]
		[InlineData(true, true, true)]
		[InlineData("yes", true, true)]
		[InlineData("true", true, true)]
		[InlineData("TRUE", true, true)]
		[InlineData("y", true, true)]

		[InlineData(0, true, false)]
		[InlineData(false, true, false)]
		[InlineData("false", true, false)]
		[InlineData("FALSE", true, false)]
		[InlineData("no", true, false)]
		[InlineData("n", true, false)]

		[InlineData("2023-01-01", false, false)]
		[InlineData("xxx", false, false)]
		[InlineData(3, false, false)]
		[InlineData(-3, false, false)]
		public void TestTryReadBoolean(object value, bool shouldRead, bool expectedResult) {
			var hasValue = CellValue.TryReadBoolean(value, out var result);
			Assert.Equal(shouldRead, hasValue);
			if (hasValue) {
				Assert.Equal(expectedResult, result);
			}
		}

		[Theory]
		[InlineData(ExcelError.ExcelErrorNull, false, null)]
		[InlineData("", true, "")]
		[InlineData("a", true, "a")]
		[InlineData(1, true, "1")]
		public void TestTryReadString(object value, bool shouldRead, string? expectedResult) {
			var hasValue = CellValue.TryReadString(value, out var result);
			Assert.Equal(shouldRead, hasValue);
			if (hasValue) {
				Assert.Equal(expectedResult, result);
			}
		}

		[Theory]
		[InlineData(ExcelError.ExcelErrorNull, "System.Int32", false, null)]
		[InlineData("", "System.String", true, "")]
		[InlineData("a", "System.String", true, "a")]
		[InlineData(1, "System.String", true, "1")]

		[InlineData(true, "System.Boolean", true, true)]
		[InlineData(false,"System.Boolean", true, false)]

		[InlineData(45226, "System.DateTime", true, "2023-10-27")]
		[InlineData(45226, "System.DateOnly", true, "2023-10-27")]

		[InlineData(1.5, "System.Int32", true, 2)]
		[InlineData(1.5, "System.Double", true, 1.5)]
		[InlineData(long.MaxValue, "System.Int64", true, long.MaxValue)]
		[InlineData("{\"Name\": \"rushui\"}", "Albatross.Excel.Test.MyClass, Albatross.Excel.Test", true, "{\"Name\": \"rushui\"}")]
		public void TestTryReadType(object value, string typeText, bool shouldRead, object? expectedResult) {
			var type = typeText.GetRequiredType();
			var hasValue = CellValue.TryReadValue(value, type, out var result);
			Assert.Equal(shouldRead, hasValue);
			if (hasValue) {
				if (type == typeof(DateOnly)) {
					expectedResult = DateOnly.Parse((string)expectedResult!);
				} else if (type == typeof(MyClass)) {
					expectedResult = JsonSerializer.Deserialize((string)expectedResult!, type);
				} else {
					expectedResult = Convert.ChangeType(expectedResult, type);
				}
				Assert.Equal(expectedResult, result);
			}
		}
	}

	public record class MyClass {
		public string Name { get; set; }

		public MyClass(string name) {
			Name = name;
		}
	}
}