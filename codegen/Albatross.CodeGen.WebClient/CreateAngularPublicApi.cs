using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.WebClient {
	public interface ICreateAngularPublicApi {
		void Generate(string outputFolder, string fileFolder, IEnumerable<TypeScriptFileDeclaration> dependancies);
	}
	public class CreateAngularPublicApi : ICreateAngularPublicApi {
		public void Generate(string outputFolder, string fileFolder, IEnumerable<TypeScriptFileDeclaration> files) {
			string publicApiFile = Path.Combine(outputFolder, "public-api.ts");
			var relativePath = Path.GetRelativePath(outputFolder, fileFolder).Replace('\\', '/');
			using (var writer = new StreamWriter(publicApiFile)) {
				foreach (var file in files) {
					string source = $"./{relativePath}/{file.Identifier.Name}";
					writer.Code(new ExportExpression {
						Source = new FileNameSourceExpression(source),
					});
				}
			}
		}
	}
}
