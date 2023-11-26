using Albatross.CodeGen.Core;
using Albatross.Text;
using System.IO;
using System.Text.Json;

namespace Albatross.CodeGen.TypeScript.Model {
	public enum ValueType {
		Variable,
		Literal,
	}
	public class TypeScriptValue : ICodeElement {
		public ValueType ValueType { get; set; }
		public object? Value { get; set; }
		public TypeScriptValue(ValueType type, object? value) {
			this.ValueType = type;
			this.Value = value;
		}

		public TextWriter Generate(TextWriter writer) {
			if (Value == null) {
				writer.Append("null");
			} else if (ValueType == ValueType.Variable) {
				writer.Append(Value);
			} else {
				var text = JsonSerializer.Serialize(Value, new JsonSerializerOptions {
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				});
				writer.Append(text);
			}
			return writer;
		}
	}
	public class TypeScriptValueArray : ICodeElement {
		public TypeScriptValueArray(params TypeScriptValue[] array) {
			Array = array;
		}

		public TypeScriptValue[] Array { get; set; }

		public TextWriter Generate(TextWriter writer) {
			if (Array.Length > 0) {
				writer.Append("{ ").WriteItems(Array, ", ", (w, item) => w.Code(item)).Append("}");
			} else {
				writer.Append("null");
			}
			return writer;
		}
	}
}
