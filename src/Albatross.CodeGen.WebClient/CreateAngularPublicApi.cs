using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.WebClient {
	public interface ICreateAngularPublicApi {
		void Generate(string outputFolder, string fileFolder, IEnumerable<TypeScriptFile> files);
	}
	public class CreateAngularPublicApi : ICreateAngularPublicApi {
		public void Generate(string outputFolder, string fileFolder, IEnumerable<TypeScriptFile> files) {
			string publicApiFile = Path.Combine(outputFolder, "public-api.ts");
			var relativePath = Path.GetRelativePath(outputFolder, fileFolder).Replace('\\', '/');
			using (var writer = new StreamWriter(publicApiFile)) {
				foreach (var file in files) {
					string source = $"./{relativePath}/{Path.GetFileNameWithoutExtension(file.Name)}";
					writer.Code(new Export(source));
				}
			}
		}
	}
}
