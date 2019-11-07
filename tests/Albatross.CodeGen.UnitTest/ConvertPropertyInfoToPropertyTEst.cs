using Albatross.CodeGen.CSharp.Conversion;
using Albatross.CodeGen.CSharp.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Albatross.CodeGen.UnitTest {
	public class ConvertPropertyInfoToPropertyTest :IClassFixture<MyTestHost>{
		private readonly MyTestHost host;

		public ConvertPropertyInfoToPropertyTest(MyTestHost host) {
			this.host = host;
		}

		public class TestClass {
			public string Text { get; set; }
			public string ReadOnlyText { get; }
			public static int Number { get; set; }
			public double Double {
				get;
				private set;
			}
		}


		public static IEnumerable<object[]> GetTestData() {
			Type type = typeof(TestClass);

			return new List<object[]> {
				new object[]{type.GetProperty(nameof(TestClass.Text)), new Property(nameof(TestClass.Text)){  Type = DotNetType.String(), CanWrite = true, CanRead = true, } },
				new object[]{type.GetProperty(nameof(TestClass.ReadOnlyText)), new Property(nameof(TestClass.ReadOnlyText)){  Type = DotNetType.String(), CanWrite = false, CanRead = true, } },
				new object[]{type.GetProperty(nameof(TestClass.Number)), new Property(nameof(TestClass.Number)){  Type = DotNetType.Integer(), CanWrite = true, CanRead = true, Static = true, } },
				new object[]{type.GetProperty(nameof(TestClass.Double)), new Property(nameof(TestClass.Double)){  Type = DotNetType.Double(), CanWrite = true, CanRead = true, SetModifier = AccessModifier.Private, } },
			};
		}

		[Theory]
		[MemberData(nameof(GetTestData))]
		public void Run(PropertyInfo propertyInfo, Property expected) {
			ConvertPropertyInfoToProperty handle = host.Provider.GetRequiredService<ConvertPropertyInfoToProperty>();
			var result = handle.Convert(propertyInfo);
			Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
		}
	}
}
