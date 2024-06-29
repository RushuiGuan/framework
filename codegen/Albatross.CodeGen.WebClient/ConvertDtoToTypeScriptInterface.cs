using Albatross.CodeGen.TypeScript.Conversions;
using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Albatross.CodeGen.WebClient {
	public interface IConvertDtoToTypeScriptInterface {
		void ConvertEnum(Type type, TypeScriptFile typeScriptFile);
		void ConvertEnum<T>(TypeScriptFile typeScriptFile) where T :struct;
		void ConvertEnums(TypeScriptFile typeScriptFile, params Assembly[] assemblies);
		void ConvertClass(Type type, TypeScriptFile typeScriptFile, IEnumerable<TypeScriptFile> dependancies);
		void ConvertClasses(TypeScriptFile typeScriptFile, IEnumerable<TypeScriptFile> dependancies, 
			Func<Type, bool>? isValidType,
			params Assembly[] assemblies);
	}

	public class ConvertDtoToTypeScriptInterface : IConvertDtoToTypeScriptInterface {
		private readonly ILogger<ConvertDtoToTypeScriptInterface> logger;
		private readonly ConvertTypeToTypeScriptInterface convertInterface;
		private readonly ConvertEnumToTypeScriptEnum convertEnum;

		public ConvertDtoToTypeScriptInterface(ILogger<ConvertDtoToTypeScriptInterface> logger, ConvertTypeToTypeScriptInterface convertInterface, 
			ConvertEnumToTypeScriptEnum convertEnum) {
			this.logger = logger;
			this.convertInterface = convertInterface;
			this.convertEnum = convertEnum;
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

		public void ConvertClasses(TypeScriptFile typeScriptFile, IEnumerable<TypeScriptFile> dependancies, 
			Func<Type, bool>? isValidType, params Assembly[] assemblies) {
			isValidType = isValidType ?? (args => true);
			foreach (var assembly in assemblies) {
				var types = assembly.GetTypes();
				foreach (Type type in types) {
					if (IsValidDtoType(type, isValidType)) {
						var item = convertInterface.Convert(type);
						typeScriptFile.InterfaceDeclarations.Add(item);
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
						typeScriptFile.EnumDeclarations.Add(convertEnum.Convert(type));
					}
				}
			}
			typeScriptFile.BuildArtifacts();
		}

		public void ConvertEnum<T>(TypeScriptFile typeScriptFile) where T : struct {
			typeScriptFile.EnumDeclarations.Add(convertEnum.Convert(typeof(T)));
		}
		public void ConvertEnum(Type enumType, TypeScriptFile typeScriptFile){
			if (enumType.IsEnum) {
				typeScriptFile.EnumDeclarations.Add(convertEnum.Convert(enumType));
			} else {
				throw new InvalidOperationException($"Class {enumType.Name} is not an enum");
			}
		}
		public void ConvertClass(Type type, TypeScriptFile typeScriptFile, IEnumerable<TypeScriptFile> dependancies) {
			var item = convertInterface.Convert(type);
			typeScriptFile.InterfaceDeclarations.Add(item);
			typeScriptFile.BuildImports(dependancies);
			typeScriptFile.BuildArtifacts();
		}
	}
}
