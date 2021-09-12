using System.IO;
using Albatross.CodeGen.CSharp.Model;
using Albatross.CodeGen.CSharp.Writer;
using Xunit;

namespace Albatross.CodeGen.UnitTest.CSharp {
	public class WriteAccessModifierTest {

		[Theory]
		[InlineData(AccessModifier.Internal, "internal")]
		[InlineData(AccessModifier.Public, "public")]
		[InlineData(AccessModifier.Private, "private")]
		[InlineData(AccessModifier.Protected, "protected")]
		[InlineData(AccessModifier.Protected | AccessModifier.Internal, "protected internal")]
		[InlineData(AccessModifier.Private | AccessModifier.Internal, "private internal")]
		public void Run(AccessModifier accessModifier, string expected) {
			StringWriter writer = new StringWriter();
			new WriteAccessModifier().Run(writer, accessModifier);
			string result = writer.ToString();
			Assert.Equal(expected, result);
		}
	}
}
