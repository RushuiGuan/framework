using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Albatross.Text;
using System.IO;
using System;

namespace Albatross.EFCore.ChangeReporting {
	public class ChangeReportDbSessionEventHandler<T> : IDbSessionEventHandler where T : class {
		public ChangeReportingOptions Options { get; } = new ChangeReportingOptions();
		public List<ChangeReport<T>> Changes { get; } = new List<ChangeReport<T>>();
		public string Text { get; private set; } = string.Empty;
		public Func<string, Task>? OnReportGenerated { get; set; }
		public const string ColumnPrefix = "Entity.";

		ChangeType ChangeType => Options.Type;
		HashSet<string> SkippedProperties => Options.SkippedProperties;

		public void PriorSave(IDbSession session) {
			foreach (var entry in session.DbContext.ChangeTracker.Entries<T>()) {
				if (entry.State == EntityState.Modified && (ChangeType & ChangeType.Modified) > 0) {
					Changes.AddRange(entry.Properties
						.Where(args => args.IsModified && !SkippedProperties.Contains(args.Metadata.Name))
						.Select(args => new ChangeReport<T>(entry.Entity, args.Metadata.Name) {
							OldValue = args.OriginalValue,
							NewValue = args.CurrentValue,
						}));
				} else if (entry.State == EntityState.Added && (ChangeType & ChangeType.Added) > 0) {
					Changes.AddRange(entry.Properties
						.Where(args => !SkippedProperties.Contains(args.Metadata.Name))
						.Select(args => new ChangeReport<T>(entry.Entity, args.Metadata.Name) {
							OldValue = null,
							NewValue = args.CurrentValue,
						}));
				} else if (entry.State == EntityState.Deleted && (ChangeType & ChangeType.Deleted) > 0) {
					Changes.AddRange(entry.Properties
						.Where(args => !SkippedProperties.Contains(args.Metadata.Name))
						.Select(args => new ChangeReport<T>(entry.Entity, args.Metadata.Name) {
							OldValue = args.CurrentValue,
							NewValue = null,
						}));
				}
			}
		}
		public async Task PostSave() {
			if (this.Changes.Any()) {
				this.Text = await BuildText(this.Options, this.Changes);
				if (!string.IsNullOrEmpty(Text) && OnReportGenerated != null) {
					await OnReportGenerated(this.Text);
				}
			}
		}

		protected virtual async Task<string> BuildText(ChangeReportingOptions options, IEnumerable<ChangeReport<T>> changes) {
			var writer = new StringWriter();
			if (options.Prefix != null) { writer.Append(options.Prefix); }
			var columns = this.Options.FixedHeaders.Select(args => $"{ColumnPrefix}{args}")
			.Union(new string[] {
					nameof(ChangeReport<object>.Property),
					nameof(ChangeReport<object>.OldValue),
					nameof(ChangeReport<object>.NewValue)
			}).ToArray();
			var option = new PrintTableOption(columns) {
				GetColumnHeader = args => (args.StartsWith(ColumnPrefix) ? args.Substring(ColumnPrefix.Length) : args).Replace(".", ""),
				FormatValue = this.FormatValue,
			};
			await writer.PrintTable(this.Changes.ToArray(), option);
			if (options.Postfix != null) {
				writer.Append(options.Postfix);
			}
			return writer.ToString();
		}
		public Dictionary<string, Func<object, object, string>> Formatters { get; } = new Dictionary<string, Func<object, object, string>>();
		public Dictionary<string, Func<object, object, Task<string>>> AsyncFormatters { get; } = new Dictionary<string, Func<object, object, Task<string>>>();
		public async Task<string> FormatValue(object? entity, string property, object? value) {
			ChangeReport<T>? report = (ChangeReport<T>)entity;
			if (report == null || value == null) {
				return string.Empty;
			} else if (property == nameof(IChangeReport.NewValue) || property == nameof(IChangeReport.OldValue)) {
				if (Formatters.TryGetValue(report.Property, out var formatter)) {
					return formatter(report, value);
				} else if (AsyncFormatters.TryGetValue(report.Property, out var asyncFormatter)) {
					return await asyncFormatter(report, value);
				}
			} else if (Formatters.TryGetValue(property, out var formatter)) {
				return formatter(report, value);
			} else if (AsyncFormatters.TryGetValue(property, out var asyncFormatter)) {
				return await asyncFormatter(report, value);
			}
			if (value is DateOnly || value is DateTime dateTime && dateTime.TimeOfDay == TimeSpan.Zero) {
				return $"{value:yyyy-MM-dd}";
			} else if (value is DateTimeOffset) {
				return $"{value:yyyy-MM-ddTHH:mm:ssz}";
			} else if (value is DateTime dateTimeValue) {
				if (dateTimeValue.Kind == DateTimeKind.Utc || report.Property.Contains("utc", StringComparison.InvariantCultureIgnoreCase)) {
					return $"{value:yyyy-MM-ddTHH:mm:ssZ}";
				} else {
					return $"{value:yyyy-MM-ddTHH:mm:ss}";
				}
			} else {
				return $"{value}";
			}
		}
	}
}
