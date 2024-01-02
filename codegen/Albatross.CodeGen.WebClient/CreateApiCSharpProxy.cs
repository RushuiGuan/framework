using Albatross.CodeGen.CSharp.Models;
using Albatross.Reflection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

#nullable enable
namespace Albatross.CodeGen.WebClient {
	public interface ICreateApiCSharpProxy {
		string Generate(string? pattern, string @namespace, IEnumerable<Assembly> assemblies, string outputDirectory, Func<Class, bool>? adjustClassModel = null);
	}
	public class CreateApiCSharpProxy : ICreateApiCSharpProxy {
		public const string DefaultPattern = "^.+Controller$";
		private readonly ConvertApiControllerToCSharpClass converter;
		private readonly ILogger<CreateApiCSharpProxy> logger;

		public CreateApiCSharpProxy(ConvertApiControllerToCSharpClass converter, ILogger<CreateApiCSharpProxy> logger) {
			this.converter = converter;
			this.logger = logger;
		}

		public string Generate(string? pattern, string @namespace, IEnumerable<Assembly> assemblies, string outputDirectory, Func<Class, bool>? adjustClassModel = null) {
			pattern = pattern ?? DefaultPattern;
			Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
			StringBuilder sb = new StringBuilder();
			using var stringWriter = new StringWriter(sb);

			foreach (var assembly in assemblies) {
				var types = assembly.GetTypes();
				foreach (Type type in types) {
					if (!type.IsAnonymousType() && !type.IsInterface && type.IsPublic) {
						if (regex.IsMatch(type.FullName ?? string.Empty)) {
							logger.LogInformation("Processing class {type}", type.FullName);
							var @class = converter.Convert(type);
							@class.Namespace = @namespace;
							if (adjustClassModel?.Invoke(@class) != false) {
								string filename = Path.Join(outputDirectory, $"{@class.Name}.Generated.cs");
								using (StreamWriter writer = new StreamWriter(filename, false)) {
									writer.Code(@class);
									stringWriter.Code(@class);
									writer.WriteLine();
								}
								logger.LogInformation("Create output file {name}", filename);
							}
						}
					}
				}
			}
			stringWriter.Flush();
			return sb.ToString();
		}
	}
}
#nullable disable