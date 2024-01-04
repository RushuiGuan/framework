using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Albatross.CodeGen.WebClient {
	public interface ICreateApiTypeScriptProxy {
		IEnumerable<TypeScriptFile> Generate(string endpoint, string? pattern, IEnumerable<Assembly> assemblies,
			IEnumerable<TypeScriptFile> dependencies, string outputDirectory, Func<Type, bool>? isValidType, Action<Class>? modifyProxyClass = null);
	}
	public class CreateApiTypeScriptProxy : ICreateApiTypeScriptProxy {
		public const string DefaultPattern = "^.+Controller$";
		private readonly ConvertApiControllerToTypeScriptClass converter;
		private readonly ILogger<CreateApiTypeScriptProxy> logger;

		public CreateApiTypeScriptProxy(ConvertApiControllerToTypeScriptClass classConverter, ILogger<CreateApiTypeScriptProxy> logger) {
			this.converter = classConverter;
			this.logger = logger;
		}

		public IEnumerable<TypeScriptFile> Generate(string endpoint, string? pattern, IEnumerable<Assembly> assemblies,
			IEnumerable<TypeScriptFile> dependancies, string outputDirectory, Func<Type, bool>? isValidType, Action<Class>? modifyProxyClass = null) {
			isValidType = isValidType ?? (args => true);
			this.converter.EndpointName = endpoint;
			pattern = pattern ?? DefaultPattern;
			Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
			List<TypeScriptFile> files = new List<TypeScriptFile>();
			foreach (var assembly in assemblies) {
				var types = assembly.GetTypes();
				foreach (Type type in types) {
					if (!type.IsAnonymousType() && !type.IsInterface && type.IsPublic) {
						if (regex.IsMatch(type.FullName ?? string.Empty)) {
							logger.LogInformation("Processing class {type}", type.FullName);
							if (isValidType(type)) {
								var @class = converter.Convert(type);
								modifyProxyClass?.Invoke(@class);
								TypeScriptFile file = new TypeScriptFile(GetApiFileName(@class.Name));
								files.Add(file);
								file.Classes.Add(@class);
								file.BuildImports(dependancies.ToArray());
								file.Imports.AddRange(@class.Imports);
								string filename = Path.Join(outputDirectory, file.Name);
								using (StreamWriter writer = new StreamWriter(filename, false)) {
									writer.Code(file);
									writer.WriteLine();
								}
								logger.LogInformation("Create output file {name}", filename);
							}
						}
					}
				}
			}
			return files;
		}

		public string GetApiFileName(string className) {
			const string postFix = "Service";
			if (className.EndsWith(postFix)) {
				StringBuilder sb = new StringBuilder();
				className = className.Substring(0, className.Length - postFix.Length);
				for (int i = 0; i < className.Length; i++) {
					char c = className[i];
					if (i > 0 && char.IsUpper(c)) {
						if (!char.IsUpper(className[i - 1]) || i != className.Length - 1 && !char.IsUpper(className[i + 1])) {
							sb.Append('-');
						}
					}
					sb.Append(char.ToLower(c));
				}
				sb.Append(".service.ts");
				return sb.ToString();
			} else {
				throw new Exception($"Api class name has to end with {postFix}");
			}
		}
	}
}