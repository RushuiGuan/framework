using Albatross.CodeGen.Core;
using Albatross.Collections;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.CSharp.Model {
	public static class Extensions {
		public static void WriteAttributes(this TextWriter writer, IEnumerable<MethodCall> attributes) =>
			attributes.ForEach(args => writer.OpenSquareBracket().Code(args).CloseSquareBracket().WriteLine());
	}
}
