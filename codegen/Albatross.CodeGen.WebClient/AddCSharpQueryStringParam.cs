using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.Text;
using System;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.WebClient {
	public class AddCSharpQueryStringParam : ICodeElement {
		public AddCSharpQueryStringParam(string key, string valueVariableName, Type valueType) {
			Key = key;
			ValueVariableName = valueVariableName;
			ValueType = valueType;
		}

		public string Key { get; set; }
		public string ValueVariableName { get; set; }
		public Type ValueType { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append("queryString.Add(").Code(new StringLiteral(Key)).Comma().Space();
			if (ValueType == typeof(DateOnly) || ValueType == typeof(DateOnly?)) {
				writer.WriteLine($"string.Format(\"{{0:yyyy-MM-dd}}\", @{ValueVariableName}));");
			} else if (ValueType == typeof(DateTime) || ValueType == typeof(DateTime?)) {
				if (ValueVariableName?.EndsWith("date", StringComparison.InvariantCultureIgnoreCase) == true) {
					writer.WriteLine($"string.Format(\"{{0:yyyy-MM-dd}}\", @{ValueVariableName}));");
				} else {
					writer.WriteLine($"string.Format(\"{{0:o}}\", @{ValueVariableName}));");
				}
			} else if (ValueType != typeof(string)) {
				writer.WriteLine($"System.Convert.ToString(@{ValueVariableName}));");
			} else {
				writer.WriteLine($"@{ValueVariableName});");
			}
			return writer;
		}
	}
}
