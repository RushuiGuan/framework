using Albatross.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Albatross.Repository.ByEFCore {
	public static class ServiceExtension {
		public static IEnumerable<IBuildEntityModel> GetEntityModels(this Assembly assembly) {
			List<IBuildEntityModel> list = new List<IBuildEntityModel>();
			foreach (Type type in assembly.GetConcreteClasses<IBuildEntityModel>()) {
				list.Add((IBuildEntityModel)Activator.CreateInstance(type)!);
			}
			return list;
		}

		public static PropertyBuilder<DateTime> UtcDateTimeProperty(this PropertyBuilder<DateTime> builder) {
			builder.HasConversion(value => value, value => DateTime.SpecifyKind(value, DateTimeKind.Utc));
			return builder;
		}

		public static PropertyBuilder<DateTime?> UtcDateTimeProperty(this PropertyBuilder<DateTime?> builder) {
			builder.HasConversion(value => value, item => item.HasValue? DateTime.SpecifyKind(item.Value, DateTimeKind.Utc): null);
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
