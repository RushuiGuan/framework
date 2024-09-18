using System.IO;
using System.Text.RegularExpressions;

namespace Albatross.CodeGen.WebClient.CSharpOld {
	public class AddCSharpRouteUrl : ICodeElement {
		private readonly string? template;

		public AddCSharpRouteUrl(string? template) {
			this.template = template;
		}

		public readonly static Regex ParamRegex = new Regex(@"{(([a-z_]+[a-z0-9_]*)?date[0-9]*)}|{([\*]{0,2})([a-z_]+[a-z0-9_]*)}", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

		public TextWriter Generate(TextWriter writer) {
			writer.Write("string path = $\"{ControllerPath}");
			if (!string.IsNullOrEmpty(template)) {
				foreach (var item in template.Split('/')) {
					writer.Write('/');
					int pos = 0;
					Match match;
					for (match = ParamRegex.Match(item); match.Success; match = match.NextMatch()) {
						writer.Write(item.Substring(pos, match.Index - pos));
						writer.Write("{");
						if (!string.IsNullOrEmpty(match.Groups[1].Value)) {
							writer.Write(match.Groups[1]);
							writer.Write(":yyyy-MM-dd");
						} else {
							writer.Write(match.Groups[4].Value);
						}
						writer.Write("}");
						pos = match.Index + match.Length;
					}
					writer.Write(item.Substring(pos));
				}
			}
			writer.Write("\";");
			return writer;
		}
	}
}