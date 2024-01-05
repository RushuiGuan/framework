using Albatross.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Albatross.EFCore.ChangeReporting {
	public static class Extensions {
		const string ColumnPrefix = "Entity.";
		public static void GetChanges<T>(this EntityEntry<T> entry, List<IChangeReport> changes, ChangeType type, string[] skippingProperties) where T : class {
			if (entry.State == EntityState.Modified && (type & ChangeType.Modified) > 0) {
				changes.AddRange(entry.Properties
					.Where(args => args.IsModified && !skippingProperties.Contains(args.Metadata.Name))
					.Select(args => new ChangeReport<T>(entry.Entity, args.Metadata.Name) {
						OriginalValue = args.OriginalValue,
						CurrentValue = args.CurrentValue,
					}));
			} else if (entry.State == EntityState.Added && (type & ChangeType.Added) > 0) {
				changes.AddRange(entry.Properties
					.Where(args => !skippingProperties.Contains(args.Metadata.Name))
					.Select(args => new ChangeReport<T>(entry.Entity, args.Metadata.Name) {
						OriginalValue = null,
						CurrentValue = args.CurrentValue,
					}));
			} else if (entry.State == EntityState.Deleted && (type & ChangeType.Deleted) > 0) {
				changes.AddRange(entry.Properties
					.Where(args => !skippingProperties.Contains(args.Metadata.Name))
					.Select(args => new ChangeReport<T>(entry.Entity, args.Metadata.Name) {
						OriginalValue = args.CurrentValue,
						CurrentValue = null,
					}));
			}
		}
		public static async Task GetChangeText(this TextWriter writer, IEnumerable<IChangeReport> changes, PrintOption.FormatValueDelegate? formatValueFunc = null, params string[] properties) {
			var columns = properties.Select(args => $"{ColumnPrefix}{args}").Union(new string[] { nameof(ChangeReport<object>.Property), nameof(ChangeReport<object>.OriginalValue), nameof(ChangeReport<object>.CurrentValue) }).ToArray();
			var option = new PrintTableOption(columns) {
				GetColumnHeader = args => (args.StartsWith(ColumnPrefix) ? args.Substring(ColumnPrefix.Length) : args).Replace(".", ""),
				FormatValue = formatValueFunc ?? PrintOption.DefaultFormatValue,
			};
			await writer.PrintTable(changes.ToArray(), option);
		}
		public static async Task GetChangeText<T>(this EntityEntry<T> entry, TextWriter writer, ChangeReportingOptions options) where T : class {
			var changes = new List<IChangeReport>();
			entry.GetChanges(changes, options.Type, options.SkippedProperties);
			await writer.GetChangeText(changes, options.FormatValueFunc, options.Properties.ToArray());
		}
		public static async Task<ChangeReportingResult> SaveAndAuditChanges<T>(this DbContext dbContext, ChangeReportingOptions options, string? user) where T : class {
			try {
				var result = new ChangeReportingResult();
				foreach (var entry in dbContext.ChangeTracker.Entries<T>()) {
					if (entry.State == EntityState.Modified) {
						result.HasAnyChanges = true;
						if (entry.Entity is IModifiedBy audit1 && !string.IsNullOrEmpty(user)) { audit1.ModifiedBy = user; }
						if (entry.Entity is IModifiedUtc audit2) { audit2.ModifiedUtc = DateTime.UtcNow; }
					}else if (entry.State == EntityState.Added) {
						result.HasAnyChanges = true;
						if (entry.Entity is ICreatedBy audit1 && !string.IsNullOrEmpty(user)) { audit1.CreatedBy = user; }
						if (entry.Entity is ICreatedUtc audit2) { audit2.CreatedUtc = DateTime.UtcNow; }
					}else if(entry.State == EntityState.Deleted) {
						result.HasAnyChanges = true;
					}
					entry.GetChanges(result.Changes, options.Type, options.SkippedProperties);
				}
				var writer = new StringWriter();
				if (options.Prefix != null) { writer.Append(options.Prefix); }
				await writer.GetChangeText(result.Changes, options.FormatValueFunc, options.Properties.ToArray());
				if (options.Postfix != null) { writer.Append(options.Postfix); }
				if (options.DoNotSave) {
					writer.WriteLine();
					writer.Append("*Changes not saved*");
				}
				result.Text = writer.ToString();
				return result;
			} finally {
				if (!options.DoNotSave) {
					await dbContext.SaveChangesAsync();
				}
			}
		}
	}
}