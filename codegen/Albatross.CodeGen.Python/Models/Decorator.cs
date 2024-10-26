using Albatross.Collections;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Decorator : CompositeModuleCodeElement {
		public Decorator(string name, params IModuleCodeElement[] parameters) : this(name, string.Empty, parameters) { }
		public Decorator(string name, string module, params IModuleCodeElement[] parameters) : base(name, module) {
			parameters.ForEach(p => AddParameter(p));
		}
		public IEnumerable<IModuleCodeElement> Parameters => Collection<IModuleCodeElement>(nameof(Parameters));
		public void AddParameter(IModuleCodeElement parameter) => AddCodeElement(parameter, nameof(Parameters));
		public void RemoveParameter(IModuleCodeElement parameter) => RemoveCodeElement(parameter, nameof(Parameters));

		public override TextWriter Generate(TextWriter writer) {
			writer.AppendLine().Append("@").Append(Name).OpenParenthesis()
				.WriteItems(Parameters, ", ", (w, args) => w.Code(args))
				.CloseParenthesis();
			return writer;
		}
	}
}