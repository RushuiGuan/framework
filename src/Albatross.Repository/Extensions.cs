using Albatross.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Albatross.Repository {
	public static class Extensions {
		static JsonSerializerOptions jsonColumnSerializationOptions = new JsonSerializerOptions {
			DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
		};
		public static ValueConverter<T, string> GetJsonValueConverter<T>(this IBuildEntityModel _, Func<T> getDefault) {
			return new ValueConverter<T, string>(
				args => JsonSerializer.Serialize(args, jsonColumnSerializationOptions),
				args => JsonSerializer.Deserialize<T>(args, jsonColumnSerializationOptions) ?? getDefault()
			);
		}

		public static void ValidateByDataAnnotations(this object entity) {
			Validator.ValidateObject(entity, new ValidationContext(entity), true);
		}

		public static IEnumerable<IBuildEntityModel> GetEntityModels(this Assembly assembly, string? namespacePrefix) {
			List<IBuildEntityModel> list = new List<IBuildEntityModel>();
			foreach (Type type in assembly.GetConcreteClasses<IBuildEntityModel>()) {
				if (string.IsNullOrEmpty(namespacePrefix) || type?.FullName?.StartsWith(namespacePrefix) == true) {
					list.Add((IBuildEntityModel)Activator.CreateInstance(type)!);
				}
			}
			return list;
		}

		public static PropertyBuilder<DateTime> UtcDateTimeProperty(this PropertyBuilder<DateTime> builder) {
			builder.HasConversion(value => value, value => DateTime.SpecifyKind(value, DateTimeKind.Utc));
			return builder;
		}

		public static PropertyBuilder<DateTime?> UtcDateTimeProperty(this PropertyBuilder<DateTime?> builder) {
			builder.HasConversion(value => value, item => item.HasValue ? DateTime.SpecifyKind(item.Value, DateTimeKind.Utc) : null);
			return builder;
		}

		public static PropertyBuilder<DateTime> DateOnlyProperty(this PropertyBuilder<DateTime> builder) {
			builder.HasConversion(value => value, value => DateTime.SpecifyKind(value, DateTimeKind.Unspecified));
			return builder;
		}

		public static PropertyBuilder<DateTime?> DateOnlyProperty(this PropertyBuilder<DateTime?> builder) {
			builder.HasConversion(value => value, item => item.HasValue ? DateTime.SpecifyKind(item.Value, DateTimeKind.Unspecified) : null);
			return builder;
		}
	}
}
