﻿using Albatross.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class ParameterCollection : ICodeElement, IEnumerable<Parameter>{
		public ParameterCollection(IEnumerable<Parameter> parameters) {
			Parameters = new List<Parameter>(parameters);
		}

		public List<Parameter> Parameters { get; }

		public TextWriter Generate(TextWriter writer) {
			writer.WriteItems(Parameters, ", ", (w, item) => w.Code(item));
			return writer;
		}

		public void InsertSelfWhenMissing() {
			if (!Parameters.Any() || Parameters.First().Name != My.Keywords.Self) {
				Parameters.Insert(0, new Parameter(My.Keywords.Self));
			}
		}

		IEnumerator<Parameter> IEnumerable<Parameter>.GetEnumerator() => this.Parameters.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => this.Parameters.GetEnumerator();
	}
}
