using Albatross.CodeGen.Python.Conversions;
using Albatross.CodeGen.Python.Models;
using Albatross.Reflection;
using Microsoft.Extensions.Logging;
using System;

namespace Albatross.CodeGen.WebClient {
	public interface IConvertDtoToPythonClass {
		void ConvertEnum(Type type, PythonModule module);
		void ConvertEnum<T>(PythonModule module) where T :struct;
		void ConvertEnums(PythonModule module, params System.Reflection.Assembly[] assemblies);
		void ConvertClass(Type type, PythonModule module, bool useDataClass);
		void ConvertClasses(PythonModule module, Func<Type, bool>? isValidType, bool useDataClass, params System.Reflection.Assembly[] assemblies);
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

		public void ConvertClasses(PythonModule module,  Func<Type, bool>? isValidType, bool useDataClass, params System.Reflection.Assembly[] assemblies) {
			isValidType = isValidType ?? (args => true);
			foreach (var assembly in assemblies) {
				var types = assembly.GetTypes();
				foreach (Type type in types) {
					if (IsValidDtoType(type, isValidType)) {
						var item = convertType.Convert(type);
						item.UseDataClass = useDataClass;
						module.AddClass(item);
					}
				}
			}
		}

		public void ConvertEnums(PythonModule module, params System.Reflection.Assembly[] assemblies) {
			foreach (var assembly in assemblies) {
				var types = assembly.GetTypes();
				foreach (Type type in types) {
					if (type.IsEnum) {
						var item = convertEnum.Convert(type);
						module.AddClass(item);
					}
				}
			}
		}
		public void ConvertEnum<T>(PythonModule module) where T : struct {
			module.AddClass(convertEnum.Convert(typeof(T)));
		}
		public void ConvertEnum(Type enumType, PythonModule module){
			if (enumType.IsEnum) {
				module.AddClass(convertEnum.Convert(enumType));
			} else {
				throw new InvalidOperationException($"Class {enumType.Name} is not an enum");
			}
		}
		public void ConvertClass(Type type, PythonModule module, bool useDataClass) { 
			var item = convertType.Convert(type);
			item.UseDataClass = useDataClass;
			module.AddClass(item);
		}
	}
}
