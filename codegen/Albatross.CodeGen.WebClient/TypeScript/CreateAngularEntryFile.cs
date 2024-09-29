using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.WebClient.Settings;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.WebClient.TypeScript {
	public interface ICreateAngularEntryFile {
		void Generate(string outputFolder, string fileFolder, IEnumerable<TypeScriptFileDeclaration> dependancies);
	}
	public class CreateAngularEntryFile : ICreateAngularEntryFile {
		public CreateAngularEntryFile(CodeGenSettings settings) {
			this.Settings = settings.TypeScriptWebClientSettings;
		}

		public TypeScriptWebClientSettings Settings { get; }

		public void Generate(string outputFolder, string fileFolder, IEnumerable<TypeScriptFileDeclaration> files) {
			string publicApiFile = Path.Combine(outputFolder, Settings.EntryFile);
			var relativePath = Path.GetRelativePath(outputFolder, fileFolder).Replace('\\', '/');
			using (var writer = new StreamWriter(publicApiFile)) {
				foreach (var file in files) {
					string source = $"./{relativePath}/{file.Name}";
					writer.Code(new ExportExpression {
						Source = new FileNameSourceExpression(source),
					});
				}
			}
		}
	}
}
