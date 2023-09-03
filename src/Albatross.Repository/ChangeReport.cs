using Albatross.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Albatross.Repository {
	public interface ICreatedBy {
		public string CreatedBy { get; set; }
	}
	public interface IModifiedBy {
		public string ModifiedBy { get; set; }
	}

	public interface ICreatedUtc {
		public DateTime CreatedUtc { get; set; }
	}
	public interface IModifiedUtc {
		public DateTime ModifiedUtc { get; set; }
	}
	[Flags]
	public enum ChangeType {
		Added = 1, Deleted = 2, Modified = 4, None = 0
	}
	public interface IChange {
		public object Entity { get; }
		public ChangeType Type { get; }
		public string Property { get; }
		public object? OriginalValue { get; }
		public object? CurrentValue { get; }
	}
	public class ChangeReport<T> : IChange where T : class {
		public T Entity { get; set; }
		public ChangeType Type { get; set; }
		public ChangeReport(T entity, string property) {
			this.Entity = entity;
			this.Property = property;
		}
		public string Property { get; set; }
		public object? OriginalValue { get; set; }
		public object? CurrentValue { get; set; }

		object IChange.Entity => this.Entity;
	}
	public class ChangeReportingOptions {
		public IEnumerable<string> Properties { get; set; } = Array.Empty<string>();
		public ChangeType Type { get; set; } = ChangeType.Modified;
		public PrintOption.FormatValueDelegate? FormatValueFunc { get; set; }
		public string? Prefix { get; set; }
		public string? Postfix { get; set; }
	}
	public class ChangeReportingResult {
		public bool HasChange => Changes.Any();
		public IEnumerable<IChange> Changes { get; set; } = Array.Empty<IChange>();
		public string Text { get; set; } = string.Empty;
	}
	public static class ChangeExtensions {
		const string ColumnPrefix = "Entity.";
		public static void GetChanges<T>(this EntityEntry<T> entry, List<ChangeReport<T>> changes, ChangeType type) where T : class {
			if (entry.State == EntityState.Added && (type & ChangeType.Added) > 0
				|| entry.State == EntityState.Modified && (type & ChangeType.Modified) > 0
				|| entry.State == EntityState.Deleted && (type & ChangeType.Deleted) > 0) {
				changes.AddRange(entry.Properties.Where(args => args.IsModified).Select(args => new ChangeReport<T>(entry.Entity, args.Metadata.Name) {
					OriginalValue = args.OriginalValue,
					CurrentValue = args.CurrentValue,
				}));
			}
		}
		public static async Task GetChangeText<T>(this TextWriter writer, IEnumerable<ChangeReport<T>> changes, PrintOption.FormatValueDelegate? formatValueFunc = null, params string[] additionalProperties) where T : class {
			var columns = additionalProperties.Select(args => $"{ColumnPrefix}{args}").Union(new string[] { nameof(ChangeReport<object>.Property), nameof(ChangeReport<object>.OriginalValue), nameof(ChangeReport<object>.CurrentValue) }).ToArray();
			var option = new PrintTableOption(columns) {
				GetColumnHeader = args => args.StartsWith(ColumnPrefix) ? args.Substring(ColumnPrefix.Length) : args,
				FormatValue = formatValueFunc ?? PrintOption.DefaultFormatValue,
			};
			await writer.PrintTable(changes.ToArray(), option);
		}
		public static async Task GetChangeText<T>(this EntityEntry<T> entry, TextWriter writer, ChangeReportingOptions options) where T : class {
			var changes = new List<ChangeReport<T>>();
			entry.GetChanges(changes, options.Type);
			await writer.GetChangeText(changes, options.FormatValueFunc, options.Properties.ToArray());
		}
		public static async Task<ChangeReportingResult> SaveAndAuditChanges<T>(this IDbSession session, ChangeReportingOptions options, string? user) where T : class {
			try {
				var changes = new List<ChangeReport<T>>();
				foreach (var entry in session.DbContext.ChangeTracker.Entries<T>()) {
					if (entry.State == EntityState.Modified) {
						if (entry.Entity is IModifiedBy audit1 && !string.IsNullOrEmpty(user)) { audit1.ModifiedBy = user; }
						if (entry.Entity is IModifiedUtc audit2) { audit2.ModifiedUtc = DateTime.UtcNow; }
					}
					if (entry.State == EntityState.Added) {
						if (entry.Entity is ICreatedBy audit1 && !string.IsNullOrEmpty(user)) { audit1.CreatedBy = user; }
						if (entry.Entity is ICreatedUtc audit2) { audit2.CreatedUtc = DateTime.UtcNow; }
					}
					entry.GetChanges(changes, options.Type);
				}
				var writer = new StringWriter();
				if (options.Prefix != null) { writer.Append(options.Prefix); }
				await writer.GetChangeText(changes, options.FormatValueFunc, options.Properties.ToArray());
				if (options.Postfix != null) { writer.Append(options.Postfix); }
				var result = new ChangeReportingResult {
					Changes = changes,
					Text = writer.ToString(),
				};
				return result;
			} finally {
				await session.SaveChangesAsync();
			}
		}
	}
}