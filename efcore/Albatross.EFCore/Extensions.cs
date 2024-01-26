using Albatross.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.EFCore {
	public static class Extensions {
		/// <summary>
		/// This method maps a text column to a Json property.  It will create a non unicode column with max length. varchar(max).
		/// It is recommended to use a sql utf8 collation
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static PropertyBuilder<TProperty> HasJsonProperty<TProperty>(this PropertyBuilder<TProperty> builder, Func<TProperty> getDefault) {
			return builder.IsUnicode(false).HasConversion(new ValueConverter<TProperty, string>(
								args => JsonSerializer.Serialize(args, EFCoreJsonOption.DefaultOptions),
								args => JsonSerializer.Deserialize<TProperty>(args, EFCoreJsonOption.DefaultOptions) ?? getDefault()));
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

		public static void EnsureNavigationProperty([NotNull] this object? value, params string[] propertyNames) {
			if (value == null) {
				throw new MissingNavigationPropertyException(propertyNames);
			}
		}
		public static async Task<HashSet<T>> ToHashSetAsync<T>(this IQueryable<T> query) {
			var array = await query.ToArrayAsync();
			return new HashSet<T>(array);
		}
	}
}
