using Albatross.Collections;
using Albatross.Text;
using System;
using System.Threading.Tasks;

namespace Albatross.EFCore.ChangeReporting {
	public static class Extensions {
		public static ChangeReportBuilder<T> ChangeType<T>(this ChangeReportBuilder<T> builder, ChangeType type) where T : class
			=> builder.Set(handler => handler.Options.Type = type);

		public static ChangeReportBuilder<T> FixedHeaders<T>(this ChangeReportBuilder<T> builder, params string[] properties) where T : class
			=> builder.Set(handler => handler.Options.FixedHeaders.AddRange(properties));

		public static ChangeReportBuilder<T> IgnoreProperties<T>(this ChangeReportBuilder<T> builder, params string[] properties) where T : class
			=> builder.Set(handler => handler.Options.SkippedProperties.AddRange(properties));

		public static ChangeReportBuilder<T> OnReportGenerated<T>(this ChangeReportBuilder<T> builder, Func<string, Task> func) where T : class
			=> builder.Set(handler => handler.OnReportGenerated = func);

		public static ChangeReportBuilder<T> ExcludeAuditProperties<T>(this ChangeReportBuilder<T> builder) where T : class
			=> builder.IgnoreProperties(nameof(IModifiedBy.ModifiedBy),
				nameof(IModifiedUtc.ModifiedUtc),
				nameof(ICreatedBy.CreatedBy),
				nameof(ICreatedUtc.CreatedUtc));

		public static ChangeReportBuilder<T> ExcludeTemporalProperties<T>(this ChangeReportBuilder<T> builder) where T : class
			=> builder.IgnoreProperties("PeriodStart", "PeriodEnd");

		public static ChangeReportBuilder<T> Deleted<T>(this ChangeReportBuilder<T> builder) where T : class
			=> builder.ChangeType(ChangeReporting.ChangeType.Deleted);

		public static ChangeReportBuilder<T> Modified<T>(this ChangeReportBuilder<T> builder) where T : class
			=> builder.ChangeType(ChangeReporting.ChangeType.Modified);

		public static ChangeReportBuilder<T> Added<T>(this ChangeReportBuilder<T> builder) where T : class
			=> builder.ChangeType(ChangeReporting.ChangeType.Added);

		public static ChangeReportBuilder<T> AllChangeTypes<T>(this ChangeReportBuilder<T> builder) where T : class
			=> builder.ChangeType(ChangeReporting.ChangeType.All);

		public static ChangeReportBuilder<T> Prefix<T>(this ChangeReportBuilder<T> builder, string prefix) where T : class
			=> builder.Set(handler => handler.Options.Prefix = prefix);

		public static ChangeReportBuilder<T> Postfix<T>(this ChangeReportBuilder<T> builder, string postfix) where T : class
			=> builder.Set(handler => handler.Options.Postfix = postfix);

		public static ChangeReportBuilder<T> AsyncFormatter<T>(this ChangeReportBuilder<T> builder, string property, Func<object?, object?, Task<string>> func) where T : class
			=> builder.Set(handler => handler.AsyncFormatters.Add(property, func));

		public static ChangeReportBuilder<T> Formatter<T>(this ChangeReportBuilder<T> builder, string property, Func<object?, object?, string> func) where T : class
			=> builder.Set(handler => handler.Formatters.Add(property, func));

		public static ChangeReportBuilder<T> Format<T>(this ChangeReportBuilder<T> builder, string property, string format) where T : class
			=> builder.Formatter(property, (entity, value) => string.Format($"{{0:{format}}}", value));
		
		public static ChangeReportBuilder<T> FormatFixedHeader<T>(this ChangeReportBuilder<T> builder, string property, string format) where T : class
			=> builder.Formatter(ChangeReportDbSessionEventHandler<T>.ColumnPrefix + property, (entity, value) => string.Format($"{{0:{format}}}", value));

		public static ChangeReportBuilder<T> NumericFormat<T>(this ChangeReportBuilder<T> builder, string property) where T : class
			=> builder.Format(property, "#,#0");

		public static ChangeReportBuilder<T> DateFormat<T>(this ChangeReportBuilder<T> builder, string property) where T : class
			=> builder.Format(property, "yyyy-MM-dd");

		public static ChangeReportBuilder<T> TimeFormat<T>(this ChangeReportBuilder<T> builder, string property) where T : class
			=> builder.Format(property, "HH:mm");
	}
}
