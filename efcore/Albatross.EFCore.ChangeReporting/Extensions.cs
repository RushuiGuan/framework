using Albatross.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.EFCore.ChangeReporting {
	public static class Extensions {
		public static ChangeReportBuilder<T> IncludeType<T>(this ChangeReportBuilder<T> builder, ChangeType type) where T : class
			=> builder.Set(handler => handler.Options.Type = type);

		public static ChangeReportBuilder<T> IncludeProperties<T>(this ChangeReportBuilder<T> builder, params string[] properties) where T : class
			=> builder.Set(handler => handler.Options.Properties.AddRange(properties));

		public static ChangeReportBuilder<T> ExcludeProperties<T>(this ChangeReportBuilder<T> builder, params string[] properties) where T : class
			=> builder.Set(handler => handler.Options.SkippedProperties.AddRange(properties));

		public static ChangeReportBuilder<T> UseSendFunc<T>(this ChangeReportBuilder<T> builder, Func<string, Task> func) where T : class
			=> builder.Set(handler => handler.SendReport = func);

		public static ChangeReportBuilder<T> ExcludeAuditProperties<T>(this ChangeReportBuilder<T> builder) where T : class
			=> builder.ExcludeProperties(nameof(IModifiedBy.ModifiedBy),
				nameof(IModifiedUtc.ModifiedUtc),
				nameof(ICreatedBy.CreatedBy),
				nameof(ICreatedUtc.CreatedUtc));

		public static ChangeReportBuilder<T> ExcludeTemporalProperties<T>(this ChangeReportBuilder<T> builder) where T : class
			=> builder.ExcludeProperties("PeriodStart", "PeriodEnd");

		public static ChangeReportBuilder<T> IncludeDeleted<T>(this ChangeReportBuilder<T> builder) where T : class
			=> builder.IncludeType(ChangeType.Deleted);

		public static ChangeReportBuilder<T> IncludeModified<T>(this ChangeReportBuilder<T> builder) where T : class
			=> builder.IncludeType(ChangeType.Modified);

		public static ChangeReportBuilder<T> IncludeAdded<T>(this ChangeReportBuilder<T> builder) where T : class
			=> builder.IncludeType(ChangeType.Added);

		public static ChangeReportBuilder<T> IncludeAll<T>(this ChangeReportBuilder<T> builder) where T : class
			=> builder.IncludeType(ChangeType.All);

		public static ChangeReportBuilder<T> UsePrefix<T>(this ChangeReportBuilder<T> builder, string prefix) where T : class
			=> builder.Set(handler => handler.Options.Prefix = prefix);

		public static ChangeReportBuilder<T> UsePostfix<T>(this ChangeReportBuilder<T> builder, string postfix) where T : class
			=> builder.Set(handler => handler.Options.Postfix = postfix);
	}
}
