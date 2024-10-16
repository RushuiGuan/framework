﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.CodeAnalysis.Test {
	public class TestClassBuilding {
		[Theory]
		[InlineData("public class Test { }", "Test")]
		public void SimpleClass(string expected, string className) {
			var node = new CodeStack()
				.Begin(new ClassDeclarationBuilder(className))
				.End().Build();
			Assert.Equal(expected, node.ToString().Replace("\r\n", " ").Trim());
		}

		const string ClassWithAttribute_Expected = @"[Test]
public class MyClass
{
}
";
		[Fact]
		public void ClassWithAttribute() {
			var node = new CodeStack().Begin(new ClassDeclarationBuilder("MyClass")).Begin(new AttributeBuilder("Test")).End().End().Build();
			Assert.Equal(ClassWithAttribute_Expected, node.ToString());
		}
		const string ClassWithMultipleAttributes_Expected = @"[Test1]
[Test2]
public class MyClass
{
}
";
		[Fact]
		public void ClassWithMultipleAttributes() {
			var node = new CodeStack().Begin(new ClassDeclarationBuilder("MyClass"))
				.Begin(new AttributeBuilder("Test1")).End()
				.Begin(new AttributeBuilder("Test2")).End()
			.End().Build();
			Assert.Equal(ClassWithMultipleAttributes_Expected, node.ToString());
		}
		const string ClassWithMultipleAttributesOfTheSameType_Expected = @"[Test(1)]
[Test(2)]
public class MyClass
{
}
";
		[Fact]
		public void ClassWithMultipleAttributesOfTheSameType() {
			var node = new CodeStack().Begin(new ClassDeclarationBuilder("MyClass"))
				.Begin(new AttributeBuilder("Test")).Begin(new AttributeArgumentListBuilder()).With(new LiteralNode(1)).End().End()
				.Begin(new AttributeBuilder("Test")).Begin(new AttributeArgumentListBuilder()).With(new LiteralNode(2)).End().End()
			.End().Build();
			Assert.Equal(ClassWithMultipleAttributesOfTheSameType_Expected, node.ToString());
		}

		const string ClassWithAttributeAndConstructorArguments_Expected = @"[Test(1, 2, 3)]
public class MyClass
{
}
";
		[Fact]
		public void ClassWithAttributeAndAttributeArguments() {
			var node = new CodeStack().Begin(new ClassDeclarationBuilder("MyClass"))
				.Begin(new AttributeBuilder("Test"))
					.Begin(new AttributeArgumentListBuilder())
						.With(new LiteralNode(1), new LiteralNode(2), new LiteralNode(3))
					.End()
				.End()
			.End().Build();
			Assert.Equal(ClassWithAttributeAndConstructorArguments_Expected, node.ToString());
		}
		const string ClassWithAttributeAndAttributeNamedArguments_Expected = @"[Test(1, 2, 3, Name = ""a"")]
public class MyClass
{
}
";
		[Fact]
		public void ClassWithAttributeAndAttributeNamedArguments() {
			var node = new CodeStack().Begin(new ClassDeclarationBuilder("MyClass"))
				.Begin(new AttributeBuilder("Test"))
					.Begin(new AttributeArgumentListBuilder())
						.With(new LiteralNode(1), new LiteralNode(2), new LiteralNode(3))
						.Begin(new AssignmentExpressionBuilder("Name"))
							.With(new LiteralNode("a"))
						.End()
					.End()
				.End()
			.End().Build();
			Assert.Equal(ClassWithAttributeAndAttributeNamedArguments_Expected, node.ToString());
		}

		const string NamespaceWithUsingDirective_Expected = @"namespace test
{
	using System;

	public interface MyInterface
	{
	}

	public class MyClass
	{
	}
}
";

		[Fact]
		public void NamespaceWithUsingDirective() {
			var node = new CodeStack().Begin(new NamespaceDeclarationBuilder("test"))
					.With(new UsingDirectiveNode("System"))
					.With(new UsingDirectiveNode("System"))
					.Begin(new ClassDeclarationBuilder("MyClass")).End()
					.Begin(new InterfaceDeclarationBuilder("MyInterface")).End()
				.End().Build();
			Assert.Equal(NamespaceWithUsingDirective_Expected, node.ToString());
		}

		const string CompilationUnitWithUsingDirective_Expected = @"using System;

public class MyTest
{
}
";
		[Fact]
		public void CompilationUnitWithUsingDirective() {
			var node = new CodeStack().Begin(new CompilationUnitBuilder())
					.With(new UsingDirectiveNode("System"))
					.Begin(new ClassDeclarationBuilder("MyTest")).End()
				.End().Build();
			Assert.Equal(CompilationUnitWithUsingDirective_Expected, node.ToString());
		}
	}
}
