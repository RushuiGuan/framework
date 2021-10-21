using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Conversion;
using Albatross.CodeGen.TypeScript.Model;
using Albatross.Reflection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Albatross.CodeGen.WebClient {
	public interface IConvertAssemblyToTypeScriptModel {
		void ConvertClasses(TypeScriptFile file, string pattern, IEnumerable<TypeScriptFile> dependancies, params Assembly[] assemblies);
	}
	public class ConvertAssemblyToTypeScriptModel  : IConvertAssemblyToTypeScriptModel {
		public const string DefaultPattern = "^.+Dto$";
		private readonly ConvertTypeToTypeScriptClass converter;
		private readonly ILogger<ConvertAssemblyToTypeScriptModel > logger;

		public ConvertAssemblyToTypeScriptModel (ConvertTypeToTypeScriptClass convertClass, ILogger<ConvertAssemblyToTypeScriptModel > logger) {
			this.converter = convertClass;
			this.logger = logger;
		}

		public void ConvertClasses(TypeScriptFile file, string pattern, IEnumerable<TypeScriptFile> dependancies, params Assembly[] assemblies) {
			pattern = pattern ?? DefaultPattern;
			Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
			foreach (var assembly in assemblies) {
				var types = assembly.GetTypes();
				foreach (Type type in types) {
					if (!type.IsAnonymousType() && !type.IsInterface && type.IsPublic && !type.IsEnum && !(type.IsAbstract && type.IsSealed)) {
						if (regex.IsMatch(type.FullName ?? string.Empty)) {
							logger.LogInformation("Processing class {type}", type.FullName);
							file.Classes.Add(Create(type));
						}
					}
				}
			}
			file.BuildImports(dependancies);
			file.BuildArtifacts();
		}
		const string DtoPostFix = "Dto";

		Class Create(Type type) {
			var result = converter.Convert(type);
			if (result.Name.EndsWith(DtoPostFix)) {
				result.Name = result.Name.Substring(0, result.Name.Length - DtoPostFix.Length);
			}
			result.Constructor = BuildConstructor(type);
			return result;
		}
		Constructor BuildConstructor(Type type) {
			var constructor = new Constructor {
				Parameters = new Parameter[] {
					new Parameter("src", new TypeScriptType(type))
				}
			};
			var ifStatement = new IfCodeBlock("src", new CodeBlock());
			constructor.Body = ifStatement;
			return constructor;
		}
	}
}
