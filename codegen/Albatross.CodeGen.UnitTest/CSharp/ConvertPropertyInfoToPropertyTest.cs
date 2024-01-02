using Albatross.CodeGen.CSharp.Conversions;
using Albatross.CodeGen.CSharp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Albatross.CodeGen.UnitTest.CSharp {
	public class ConvertPropertyInfoToPropertyTest {
		public class TestClass {
			public string? Text { get; set; }
			public string? ReadOnlyText { get; }
			public static int Number { get; set; }
			public double Double {
				get;
				private set;
			}
		}


		public static IEnumerable<object[]> GetTestData() {
			Type type = typeof(TestClass);

			return new List<object[]> {
				new object[]{type.GetProperty(nameof(TestClass.Text))!, new Property(nameof(TestClass.Text), DotNetType.String()) {   CanWrite = true, CanRead = true, } },
				new object[]{type.GetProperty(nameof(TestClass.ReadOnlyText))!, new Property(nameof(TestClass.ReadOnlyText), DotNetType.String()) {   CanWrite = false, CanRead = true, } },
				new object[]{type.GetProperty(nameof(TestClass.Number))!, new Property(nameof(TestClass.Number), DotNetType.Integer()) {   CanWrite = true, CanRead = true, Static = true, } },
				new object[]{type.GetProperty(nameof(TestClass.Double))!, new Property(nameof(TestClass.Double), DotNetType.Double()) {  CanWrite = true, CanRead = true, SetModifier = AccessModifier.Private, } },
			};
		}

		[Theory]
		[MemberData(nameof(GetTestData))]
		public void Run(PropertyInfo propertyInfo, Property expected) {
			var handle = new ConvertPropertyInfoToProperty();
			var result = handle.Convert(propertyInfo);
			Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
		}
	}
}
