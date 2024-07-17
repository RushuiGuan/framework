using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.CodeGen.WebClient.UnitTest {
	public class TestRouteConversion {
		[Theory]
		[InlineData("api/endpoint", "`api/endpoint`")]
		[InlineData("api/endpoint/{id}", "`api/endpoint/${id}`")]
		[InlineData("api/{market}/{id}", "`api/${market}/${id}`")]
		public void TestConversion(string input, string expected ) {
			var expression = TypeScript.Extensions.ConvertRoute2StringInterpolation(input);
			var result = new StringWriter().Code(expression).ToString();
			Assert.Equal(expected, result);
		}
	}
}
