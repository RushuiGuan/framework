using Albatross.CodeGen.Core;
using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.CSharp.Model {
	public class Property : ICodeElement {
		public Property(string name, DotNetType type) {
			this.Name = name;
			this.Type = type;
		}

		public string Name { get; set; }
		public DotNetType Type { get; set; }
		public AccessModifier Modifier { get; set; } = AccessModifier.Public;
		public AccessModifier SetModifier { get; set; } = AccessModifier.Public;
		public bool Static { get; set; }
		public bool CanWrite { get; set; } = true;
		public bool CanRead { get; set; } = true;
		public ICodeElement? GetCodeBlock { get; set; }
		public ICodeElement? SetCodeBlock { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Code(new AccessModifierElement(Modifier)).Space();
			if (Static) { writer.Static(); }
			writer.Code(Type).Space().Append(Name);

			using (var scope = writer.BeginScope()) {
				if (CanRead) {
					if (GetCodeBlock != null) {
						using var subScope = scope.Writer.BeginScope("get");
						subScope.Writer.Code(GetCodeBlock);
					} else {
						scope.Writer.Append("get;");
					}
				}
				if (CanWrite) {
					if (SetModifier != Modifier) {
						scope.Writer.Code(new AccessModifierElement(SetModifier)).Space();
					}
					if (SetCodeBlock != null) {
						using var subScope = scope.Writer.BeginScope("set");
						subScope.Writer.Code(SetCodeBlock);
					} else {
						scope.Writer.Write("set;");
					}
				}
			}
			return writer;
		}
	}
}