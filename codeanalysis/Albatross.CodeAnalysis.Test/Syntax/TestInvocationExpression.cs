using Albatross.CodeAnalysis.Syntax;
using FluentAssertions;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestInvocationExpression {
		const string MethodInvocation_Expected = @"test(1, 2, 3)
";
		[Fact]
		public void MethodInvocation() {
			var result = new CodeStack().Begin(new InvocationExpressionBuilder("test"))
					.Begin(new ArgumentListBuilder())
						.With(new LiteralNode(1), new LiteralNode(2), new LiteralNode(3))
					.End()
				.End().Build();
			Assert.Equal(MethodInvocation_Expected, result.ToString());
		}

		const string MethodInvocationWithMemberAccess_Expected = @"this.test(1, 2, 3)
";
		[Fact]
		public void MethodInvocationWithMemberAccess() {
			var result = new CodeStack()
				.With(new ThisExpression()).With(new IdentifierNode("test"))
				.To(new MemberAccessBuilder())
				.ToNewBegin(new InvocationExpressionBuilder())
					.Begin(new ArgumentListBuilder())
						.With(new LiteralNode(1), new LiteralNode(2), new LiteralNode(3))
					.End()
				.End().Build();
			Assert.Equal(MethodInvocationWithMemberAccess_Expected, result.ToString());
		}

		const string ChainedMethodInvocation_Expected = @"service.InvokeA().InvokeB().InvokeC()
";
		[Fact]
		public void ChainedMethodInvocation() {
			var result = new CodeStack()
				.With(new IdentifierNode("service"))
				.With(new IdentifierNode("InvokeA"))
				.To(new MemberAccessBuilder())
				.To(new InvocationExpressionBuilder())
				.With(new IdentifierNode("InvokeB"))
				.To(new MemberAccessBuilder())
				.To(new InvocationExpressionBuilder())
				.To(new MemberAccessBuilder())
				.With(new IdentifierNode("InvokeC"))
				.To(new MemberAccessBuilder())
				.To(new InvocationExpressionBuilder())
				.Build();
			result.Should().Be(ChainedMethodInvocation_Expected);
		}

		const string ChainPropertyInvocation_Expected = @"service.InvokeA.InvokeB.InvokeC
";
		[Fact]
		public void ChainPropertyInvocation() {
			var result = new CodeStack()
				.With(new IdentifierNode("service"))
				.With(new IdentifierNode("InvokeA"))
				.With(new IdentifierNode("InvokeB"))
				.With(new IdentifierNode("InvokeC"))
				.To(new MemberAccessBuilder())
				.Build();
			result.Should().Be(ChainPropertyInvocation_Expected);
		}

		const string ChainPropertyAndMethodInvocation_Expected = @"service.InvokeA().MyProperty.InvokeC()
";
		[Fact]
		public void ChainPropertyAndMethodInvocation() {
			var result = new CodeStack()
				.With(new IdentifierNode("service"))
				.With(new IdentifierNode("InvokeA"))
				.To(new InvocationExpressionBuilder())
				.With(new IdentifierNode("MyProperty"))
				.To(new InvocationExpressionBuilder("InvokeC"))
				.Build();
			result.Should().Be(ChainPropertyAndMethodInvocation_Expected);
		}
	}
}