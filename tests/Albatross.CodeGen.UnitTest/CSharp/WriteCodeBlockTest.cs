using System.IO;
using Albatross.CodeGen.CSharp.Model;
using Albatross.CodeGen.CSharp.Writer;
using Albatross.CodeGen.Core;
using Xunit;

namespace Albatross.CodeGen.UnitTest.CSharp {
	public class WriteCodeBlockTest {
		[Theory]
		[InlineData("test;", " {\n\ttest;\n}")]
		public void Run(string input, string expected) {
			StringWriter writer = new StringWriter();
			writer.Run(new WriteCodeBlock(), new CodeBlock(input));
			string actual = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(expected, actual);
		}
	}
}
