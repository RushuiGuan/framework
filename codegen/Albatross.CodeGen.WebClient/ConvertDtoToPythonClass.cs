using Albatross.CodeGen.Python.Conversions;
using Albatross.CodeGen.Python.Models;
using Albatross.Reflection;
using Microsoft.Extensions.Logging;
using System;

namespace Albatross.CodeGen.WebClient {
	public interface IConvertDtoToPythonClass {
		void ConvertEnum(Type type, Module module);
		void ConvertEnum<T>(Module module) where T :struct;
		void ConvertEnums(Module module, params System.Reflection.Assembly[] assemblies);
		void ConvertClass(Type type, Module module);
		void ConvertClasses(Module module, Func<Type, bool>? isValidType, params System.Reflection.Assembly[] assemblies);
	}

	public class ConvertDtoToPythonClass : IConvertDtoToPythonClass {
		private readonly ILogger<ConvertDtoToPythonClass> logger;
		private readonly ConvertTypeToPythonClass convertType;
		private readonly ConvertEnumToClass convertEnum;

		public ConvertDtoToPythonClass(ILogger<ConvertDtoToPythonClass> logger, ConvertTypeToPythonClass convertType, 
			ConvertEnumToClass convertEnum) {
			this.logger = logger;
			this.convertType = convertType;
			this.convertEnum = convertEnum;
		}

		bool IsValidDtoType(Type type, Func<Type, bool> predicate) => 
			!type.IsAnonymousType() && !type.IsInterface && type.IsPublic
			&& !type.IsEnum && !(type.IsAbstract && type.IsSealed) 
			&& !type.IsDerived<Attribute>() && !type.IsDerived<Exception>()
			&& predicate(type);

		public void ConvertClasses(Module module,  Func<Type, bool>? isValidType, params System.Reflection.Assembly[] assemblies) {
			isValidType = isValidType ?? (args => true);
			foreach (var assembly in assemblies) {
				var types = assembly.GetTypes();
				foreach (Type type in types) {
					if (IsValidDtoType(type, isValidType)) {
						var item = convertType.Convert(type);
						module.Classes.Add(item);
					}
				}
			}
		}

		public void ConvertEnums(Module module, params System.Reflection.Assembly[] assemblies) {
			foreach (var assembly in assemblies) {
				var types = assembly.GetTypes();
				foreach (Type type in types) {
					if (type.IsEnum) {
						module.Classes.Add(convertEnum.Convert(type));
					}
				}
			}
		}
		public void ConvertEnum<T>(Module module) where T : struct {
			module.Classes.Add(convertEnum.Convert(typeof(T)));
		}
		public void ConvertEnum(Type enumType, Module module){
			if (enumType.IsEnum) {
				module.Classes.Add(convertEnum.Convert(enumType));
			} else {
				throw new InvalidOperationException($"Class {enumType.Name} is not an enum");
			}
		}
		public void ConvertClass(Type type, Module module) { 
			var item = convertType.Convert(type);
			module.Classes.Add(item);
		}
	}
}
