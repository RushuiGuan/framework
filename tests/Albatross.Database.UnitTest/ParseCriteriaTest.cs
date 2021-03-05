using Albatross.Database.SqlServer;
using Xunit;

namespace Albatross.Database.UnitTest
{
	public class ParseCriteriaTest {

		[Theory]
		[InlineData(null, null)]
		[InlineData("", null)]
		[InlineData("test", null)]
		[InlineData("a.b", "a")]
		[InlineData("a.*", "a")]
		[InlineData("*.*", null)]
		[InlineData("*a.*", "%a")]
		[InlineData("*a*b*.*", "%a%b%")]
		public void SchemaCheck(string criteria, string expected) {
			string schema, name;
			new ParseCriteria().Parse(criteria, out schema, out name);
			Assert.Equal(expected, schema);
		}

		[Theory]
		[InlineData(null, null)]
		[InlineData("", null)]
		[InlineData("test", "test")]
		[InlineData("a.b", "b")]
		[InlineData("a.*", null)]
		[InlineData("a.b*", "b%")]
		[InlineData("*", null)]
		[InlineData("*.*", null)]
		public void NameCheck(string criteria, string expected) {
			string schema, name;
			new ParseCriteria().Parse(criteria, out schema, out name);
			Assert.Equal(expected, name);
		}
	}
}
