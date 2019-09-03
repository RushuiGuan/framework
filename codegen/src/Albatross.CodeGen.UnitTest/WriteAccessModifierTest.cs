using System;
using System.IO;
using Albatross.CodeGen.CSharp.Model;
using Albatross.CodeGen.CSharp.Writer;
using NUnit.Framework;

namespace Albatross.CodeGen.UnitTest {
	[TestFixture(TestOf =typeof(WriteAccessModifier))]
	public class WriteAccessModifierTest {

		[TestCase(AccessModifier.Internal, ExpectedResult = "internal")]
		[TestCase(AccessModifier.Public, ExpectedResult = "public")]
		[TestCase(AccessModifier.Private, ExpectedResult = "private")]
		[TestCase(AccessModifier.Protected, ExpectedResult = "protected")]
		[TestCase(AccessModifier.Protected | AccessModifier.Internal, ExpectedResult = "protected internal")]
		[TestCase(AccessModifier.Private| AccessModifier.Internal, ExpectedResult = "private internal")]
		public string Run(AccessModifier accessModifier) {
			StringWriter writer = new StringWriter();
			new WriteAccessModifier().Run(writer, accessModifier);
			return writer.ToString();
		}
	}
}
