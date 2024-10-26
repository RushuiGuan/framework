using Albatross.Collections;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class MethodCall : CompositeModuleCodeElement {
		public MethodCall(Method method, params IModuleCodeElement[] parameters) : base(string.Empty, string.Empty) {
			Method = method;
			parameters.ForEach(AddParameter);
		}
		public MethodCall(string method, params IModuleCodeElement[] parameters) : base(string.Empty, string.Empty) {
			Method = new Method(method);
			parameters.ForEach(AddParameter);
		}

		public Method Method {
			get => Single<Method>(nameof(Method));
			set => Set(value, nameof(Method));
		}
		public IEnumerable<IModuleCodeElement> Parameters => Collection<IModuleCodeElement>(nameof(Parameters));
		public void AddParameter(IModuleCodeElement parameter) => AddCodeElement(parameter, nameof(Parameters));
		public void RemoveParameter(IModuleCodeElement parameter) => RemoveCodeElement(parameter, nameof(Parameters));

		public override TextWriter Generate(TextWriter writer) {
			writer.Append(Method.Name).OpenParenthesis()
				.WriteItems(Parameters, ", ", (w, args) => w.Code(args))
				.CloseParenthesis();
			return writer;
		}
	}
}