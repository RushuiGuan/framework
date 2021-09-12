﻿using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

#nullable enable
namespace Albatross.CodeGen.WebClient {
	public interface IGenerateApiCSharpProxy {
		string Generate(string? pattern, string @namespace, IEnumerable<Assembly> assemblies, string outputDirectory);
	}
	public class GenerateApiCSharpProxy : IGenerateApiCSharpProxy {
		public const string DefaultPattern = "^.+Controller$";
		private readonly ConvertApiControllerToCSharpClass converter;
		private readonly ICodeGenerator<Class> codegen;

		public GenerateApiCSharpProxy(ConvertApiControllerToCSharpClass converter, ICodeGenerator<Class> codegen) {
			this.converter = converter;
			this.codegen = codegen;
		}

		public string Generate(string? pattern, string @namespace, IEnumerable<Assembly> assemblies, string outputDirectory) {
			pattern = pattern ?? DefaultPattern;
			Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
			StringBuilder sb = new StringBuilder();
			using var stringWriter = new StringWriter(sb);

			foreach (var assembly in assemblies) {
				var types = assembly.GetTypes();
				foreach (Type type in types) {
					if (!type.IsAnonymousType() && !type.IsInterface && type.IsPublic) {
						if (regex.IsMatch(type.FullName ?? string.Empty)) {
							var @class = converter.Convert(type);
							@class.Namespace = @namespace;
							string filename = Path.Join(outputDirectory, $"{@class.Name}.Generated.cs");
							using (StreamWriter writer = new StreamWriter(filename, false)) {
								codegen.Run(writer, @class);
								codegen.Run(stringWriter, @class);
								writer.WriteLine();
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