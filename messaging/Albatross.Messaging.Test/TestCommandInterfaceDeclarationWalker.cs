using Albatross.CodeAnalysis.MSBuild;
using Albatross.CodeAnalysis.Symbols;
using Albatross.Messaging.CodeGen;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Messaging.Test {
	/// <summary>
	/// <see cref="Albatross.Messaging.CodeGen.MessagingCodeGenSyntaxWalker"/> should be thoroughly tested since missing targets will cause
	/// silent failures on the consumers of the generate code
	/// </summary>
	public class TestCommandInterfaceSearch {
		const string CommandInterfaceTestCase1 = @"
namespace Test {
	public class CommandInterfaceAttribute: System.Attribute {}
}
namespace Test1 {
	using Test;
	[CommandInterface]
	public partial interface ITest {
		int Id {get;}
	}
	public class TestClass: ITest {
		public int Id { get; set; }
	}
}
";
		const string CommandInterfaceTestCase2 = @"
namespace Test {
	public class CommandInterfaceAttribute: System.Attribute {}
}
namespace Test1 {
	using Test;
	// works with attribute
	[CommandInterface]
	public partial interface ITest { int Id { get; } }
	public class TestClass: ITest { public int Id { get; set; } }
	public record class TestRecord: ITest { public int Id { get; set; } }

	// doesn't work since it is not partial 
	[CommandInterface]
	public interface ITest1 {}
	public class TestClass1: ITest1 { }

	// works with the postfix of `command`, partial interface and no members
	public partial interface ITestCommand {}
	public class TestClass2: ITestCommand { }
	public record class TestClass3: ITestCommand { }
}";
		const string CommandInterfaceTestCase2_Result = "Test1.ITest:Test1.TestClass,Test1.TestRecord|Test1.ITestCommand:Test1.TestClass2,Test1.TestClass3";

		[Theory]
		// not partial class
		[InlineData(@"[Test.CommandInterface] public interface ITest {} public class TestClass: ITest {}", "")]
		// not partial class
		[InlineData(@"public interface ITestCommand {} public class TestClass: ITestCommand {}", "")]
		// Partial interface with a postfix of `command`.  But it is not an empty interface
		[InlineData(@"public partial interface ITestCommand {int Test();} public class TestClass: ITestCommand {}", "")]
		// Partial interface with an attribute of name "CommandInterfaceAttribute"
		[InlineData(CommandInterfaceTestCase1, "Test1.ITest:Test1.TestClass")]
		[InlineData(CommandInterfaceTestCase2, CommandInterfaceTestCase2_Result)]
		// Partial interface with a name with the postfix of `command` and no members
		[InlineData(@"public partial interface ITestCommand {} public class TestClass: ITestCommand {}", "ITestCommand:TestClass")]
		// match criteria above with record class
		[InlineData(@"public partial interface ITestCommand {} public record class TestClass: ITestCommand {}", "ITestCommand:TestClass")]
		// criteria above with multiple implementations
		[InlineData(@"public partial interface ITestCommand {} public class TestClass1: ITestCommand {} public class TestClass2: ITestCommand {}", "ITestCommand:TestClass1,TestClass2")]
		// has an interface with no implementation
		[InlineData(@"public partial interface ITestCommand {}", "")]
		// has implementation but no interface declaration
		[InlineData(@"public class TestClass1: ITestCommand {} public class TestClass2: ITestCommand {}", "")]
		public void TestCommandInterfaceAndImplementationSearch(string code, string expected) {
			// Arrange
			var compilation = code.CreateCompilation();
			// Act
			var result = new List<string>();
			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var walker = new MessagingCodeGenSyntaxWalker(semanticModel);
				walker.Visit(syntaxTree.GetRoot());
				foreach (var item in walker.CommandInterfaces) {
					result.Add($"{item.Key.GetFullName()}:{string.Join(",", item.Value.Select(x => x.GetFullName()))}");
				}
			}
			// Assert
			Assert.Equal(expected, string.Join("|", result));
		}
	}
}
