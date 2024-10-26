using System;
using Xunit;

namespace Albatross.Reflection.Test {
	public class TestGetSetPropertyValue {
		record class Style {
			public string? Name { get; set; }
			public string? Color { get; set; }
			public Width? Width { get; set; }
			public Padding? Padding { get; set; }
		}

		record class Width {
			public string? Unit { get; set; }
			public int Number { get; set; }
			public int? Stroke { get; set; }
		}
		record class Padding {
			public string? Left { get; set; }
			public string? Right { get; set; }
		}


		[Theory]
		[InlineData(nameof(Style.Name), "box")]
		[InlineData(nameof(Style.Color), "red")]
		[InlineData("Width.Unit", "px")]
		[InlineData("Width.Number", 100)]
		[InlineData("Width.Stroke", null)]
		[InlineData("Padding", null)]
		[InlineData("Padding.Left", null)]
		[InlineData("Padding.Right", null)]
		[InlineData("Padding.x", null)] // the method will return null here even when x is not a valid property because the Padding property is null
		public void TestGetPropertyValue(string propertyName, object? expected) {
			var obj = new Style {
				Name = "box",
				Color = "red",
				Width = new Width {
					Unit = "px",
					Number = 100,
				}
			};
			Assert.Equal(typeof(Style).GetPropertyValue(obj, propertyName, false), expected);
		}

		[Theory]
		[InlineData(nameof(Style.Name), "box")]
		[InlineData(nameof(Style.Color), "red")]
		[InlineData("width.unit", "px")]
		[InlineData("Width.number", 100)]
		[InlineData("Width.stroke", null)]
		[InlineData("padding", null)]
		[InlineData("padding.left", null)]
		[InlineData("Padding.Right", null)]
		[InlineData("Padding.x", null)] // the method will return null here even when x is not a valid property because the Padding property is null
		public void TestGetPropertyValueIgnoreCase(string propertyName, object? expected) {
			var obj = new Style {
				Name = "box",
				Color = "red",
				Width = new Width {
					Unit = "px",
					Number = 100,
				}
			};
			Assert.Equal(typeof(Style).GetPropertyValue(obj, propertyName, true), expected);
		}
		[Theory]
		[InlineData("Name1")]
		[InlineData("Color1")]
		[InlineData("Width.Unit1")]
		public void TestGetPropertyValueNotFound(string propertyName) {
			var obj = new Style {
				Name = "box",
				Color = "red",
				Width = new Width {
					Unit = "px",
					Number = 100,
				}
			};
			Assert.Throws<ArgumentException>(() => typeof(Style).GetPropertyValue(obj, propertyName, false));
		}


		[Theory]
		[InlineData("name", "box")]
		[InlineData("width.unit", "inches")]
		[InlineData("padding.left", "50px")]
		public void TestSetPropertyValue(string propertyName, object value) {
			var obj = new Style {
				Width = new Width(),
				Padding = new Padding(),
			};
			typeof(Style).SetPropertyValue(obj, propertyName, value, true);
			Assert.Equal(value, typeof(Style).GetPropertyValue(obj, propertyName, true));
		}

		[Theory]
		[InlineData("width.unit", "inches")]
		[InlineData("padding.left", "50px")]
		public void TestSetNullPropertyValueError(string propertyName, object value) {
			var obj = new Style();
			Assert.ThrowsAny<InvalidOperationException>(() => typeof(Style).SetPropertyValue(obj, propertyName, value, true));
		}

		[Theory]
		[InlineData("name", "System.String")]
		[InlineData("width.number", "System.Int32")]
		[InlineData("padding.left", "System.String")]
		public void TestGetPropertyType(string propertyName, string expected) {
			var actual = typeof(Style).GetPropertyType(propertyName, true);
			Assert.Equal(Type.GetType(expected), actual);
		}
	}
}