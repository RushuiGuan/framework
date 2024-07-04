using Albatross.Reflection;
using Albatross.CodeGen.TypeScript.Declarations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Albatross.CodeGen.TypeScript.Conversions;

namespace Albatross.CodeGen.WebClient {
	public interface ICreateTypeScriptDto {
		TypeScriptFileDeclaration Generate(IEnumerable<Assembly> assemblies, IEnumerable<Type> additionalDtoClass, IEnumerable<TypeScriptFileDeclaration> dependancies,
			string outputDirectory, string name, Func<Type, bool>? isValidType);
	}
	public class CreateTypeScriptDto : ICreateTypeScriptDto {
		private readonly ConvertEnumToTypeScriptEnum enumConverter;
		private readonly ConvertTypeToTypeScriptInterface interfaceConverter;
		private readonly ILogger<CreateTypeScriptDto> logger;

		public CreateTypeScriptDto(ConvertEnumToTypeScriptEnum enumConverter, ConvertTypeToTypeScriptInterface interfaceConverter, ILogger<CreateTypeScriptDto> logger) {
			this.enumConverter = enumConverter;
			this.interfaceConverter = interfaceConverter;
			this.logger = logger;
		}
		
		bool IsValidDtoType(Type type, Func<Type, bool> predicate) =>
			!type.IsAnonymousType()
				&& !type.IsInterface
				&& type.IsPublic
				&& !type.IsEnum
				&& !(type.IsAbstract && type.IsSealed)
				&& !type.IsDerived<Attribute>()
				&& !type.IsDerived<Exception>()
				&& !type.IsDerived<JsonConverter>()
				&& !type.IsDerived(typeof(JsonConverter<>))
				&& predicate(type);

		IEnumerable<Type> GetCandidates(IEnumerable<Assembly> assemblies, IEnumerable<Type> types, Func<Type, bool> predicate) {
			foreach(var assembly in assemblies) {
				foreach (var type in assembly.GetTypes()) {
					if (IsValidDtoType(type, predicate)) {
						yield return type;
					}
				}
			}
			foreach (var type in types) {
				if (IsValidDtoType(type, predicate)) {
					yield return type;
				}
			}
		}

		public TypeScriptFileDeclaration Generate(IEnumerable<Assembly> assemblies, IEnumerable<Type> additionalDtoClass, IEnumerable<TypeScriptFileDeclaration> dependancies,
			string outputDirectory, string name, Func<Type, bool>? isValidType) {
			string dtoFileName = System.IO.Path.Join(outputDirectory, $"{name}.ts");
			var candidates = GetCandidates(assemblies, additionalDtoClass, isValidType ?? (x => true));

			var file = new TypeScriptFileDeclaration(name) {
				EnumDeclarations = candidates.Where(x=>x.IsEnum).Select(enumConverter.Convert),
				InterfaceDeclarations = candidates.Where(x=>!x.IsEnum).Select(interfaceConverter.Convert),
			};
			using (var writer = new StreamWriter(dtoFileName, false)) {
				writer.Code(file);
			}
			return file;
		}
	}
}
