using Albatross.Collections;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.CSharp.Models {
	public static class Extensions {
		public static void WriteAttributes(this TextWriter writer, IEnumerable<MethodCall> attributes) =>
			attributes.ForEach(args => writer.OpenSquareBracket().Code(args).CloseSquareBracket().WriteLine());
	}
}