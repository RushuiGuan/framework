using Albatross.CodeGen.TypeScript.Conversion;
using Albatross.CodeGen.TypeScript.Model;
using Albatross.Reflection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Albatross.CodeGen.WebClient {
	public interface IConvertDtoToTypeScriptInterface {
		void ConvertEnums(TypeScriptFile fileparams, params Assembly[] assemblies);
		void ConvertClasses(TypeScriptFile file, string pattern,  IEnumerable<TypeScriptFile> dependancies, params Assembly[] assemblies);
	}

	public class ConvertDtoToTypeScriptInterface : IConvertDtoToTypeScriptInterface {
		public const string DefaultPattern = "^.*(?<!Exception)$";
		private readonly ILogger<ConvertDtoToTypeScriptInterface> logger;
		private readonly ConvertTypeToTypeScriptInterface convertInterface;
		private readonly ConvertEnumToTypeScriptEnum convertEnum;

		public ConvertDtoToTypeScriptInterface(ILogger<ConvertDtoToTypeScriptInterface> logger, ConvertTypeToTypeScriptInterface convertInterface, ConvertEnumToTypeScriptEnum convertEnum) {
			this.logger = logger;
			this.convertInterface = convertInterface;
			this.convertEnum = convertEnum;
		}

		public void ConvertClasses(TypeScriptFile typeScriptFile, string? pattern, IEnumerable<TypeScriptFile> dependancies, params Assembly[] assemblies) {
			pattern = pattern ?? DefaultPattern;
			Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
			foreach (var assembly in assemblies) {
				var types = assembly.GetTypes();
				foreach (Type type in types) {
					if (!type.IsAnonymousType() && !type.IsInterface && type.IsPublic && !type.IsEnum && !(type.IsAbstract && type.IsSealed)) {
						if (regex.IsMatch(type.FullName ?? string.Empty)) {
							var item = convertInterface.Convert(type);
							typeScriptFile.Interfaces.Add(item);
						}
					}
				}
			}
			typeScriptFile.BuildImports(dependancies);
			typeScriptFile.BuildArtifacts();
		}

		public void ConvertEnums(TypeScriptFile typeScriptFile, params Assembly[] assemblies) {
			foreach (var assembly in assemblies) {
				var types = assembly.GetTypes();
				foreach (Type type in types) {
					if (type.IsEnum) {
						typeScriptFile.Enums.Add(convertEnum.Convert(type));
					}
				}
			}
			typeScriptFile.BuildArtifacts();
		}
	}
}
