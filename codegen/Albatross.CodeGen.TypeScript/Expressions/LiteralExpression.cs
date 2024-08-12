using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public abstract record class LiteralExpression : SyntaxNode, IExpression {
		public override IEnumerable<ISyntaxNode> Children => [];
	}

	public record class StringLiteralExpression : LiteralExpression {
		public StringLiteralExpression(string value) {
			this.Value = value;
		}
		public string Value { get; }
		public override TextWriter Generate(TextWriter writer) {
			return writer.Append(JsonSerializer.Serialize(Value));
		}
	}
	public record class NumberLiteralExpression : LiteralExpression {
		public NumberLiteralExpression(double value) {
			this.Value = value;
		}
		public NumberLiteralExpression(object value) {
			this.Value = System.Convert.ToDouble(value);
		}
		public double Value { get; }
		public override TextWriter Generate(TextWriter writer) {
			return writer.Append(Value);
		}
	}
	public record class BooleanLiteralExpression : LiteralExpression {
		public BooleanLiteralExpression(bool value) {
			this.Value = value;
		}
		public bool Value { get; }
		public override TextWriter Generate(TextWriter writer) {
			return writer.Append(Value.ToString().ToLower());
		}
	}
	public record class NullLiteralExpression : LiteralExpression {
		public override TextWriter Generate(TextWriter writer) {
			return writer.Append("null");
		}
	}

	public record class UndefinedLiteralExpression : LiteralExpression {
		public override TextWriter Generate(TextWriter writer) {
			return writer.Append("undefined");
		}
	}

	public record class ArrayLiteralExpression : LiteralExpression {
		public ListOfSyntaxNodes<IExpression> Items { get; init; } = new ListOfSyntaxNodes<IExpression>();
		public override TextWriter Generate(TextWriter writer) {
			writer.Append("[").Code(Items).Append("]");
			return writer;
		}
		public override IEnumerable<ISyntaxNode> Children => [Items];
	}
}