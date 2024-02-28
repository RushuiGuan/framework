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
		public Func<string, Task>? SendReport { get; set; }

		ChangeType ChangeType => Options.Type;
		HashSet<string> SkippedProperties => Options.SkippedProperties;

		public void PriorSave(IDbSession session) {
			foreach (var entry in session.DbContext.ChangeTracker.Entries<T>()) {
				if (entry.State == EntityState.Modified && (ChangeType & ChangeType.Modified) > 0) {
					Changes.AddRange(entry.Properties
						.Where(args => args.IsModified && !SkippedProperties.Contains(args.Metadata.Name))
						.Select(args => new ChangeReport<T>(entry.Entity, args.Metadata.Name) {
							OriginalValue = args.OriginalValue,
							CurrentValue = args.CurrentValue,
						}));
				} else if (entry.State == EntityState.Added && (ChangeType & ChangeType.Added) > 0) {
					Changes.AddRange(entry.Properties
						.Where(args => !SkippedProperties.Contains(args.Metadata.Name))
						.Select(args => new ChangeReport<T>(entry.Entity, args.Metadata.Name) {
							OriginalValue = null,
							CurrentValue = args.CurrentValue,
						}));
				} else if (entry.State == EntityState.Deleted && (ChangeType & ChangeType.Deleted) > 0) {
					Changes.AddRange(entry.Properties
						.Where(args => !SkippedProperties.Contains(args.Metadata.Name))
						.Select(args => new ChangeReport<T>(entry.Entity, args.Metadata.Name) {
							OriginalValue = args.CurrentValue,
							CurrentValue = null,
						}));
				}
			}
		}

		protected virtual async Task<string> BuildText(ChangeReportingOptions options, IEnumerable<ChangeReport<T>> changes) {
			const string ColumnPrefix = "Entity.";
			var writer = new StringWriter();
			if(options.Prefix != null) { writer.Append(options.Prefix); }
			var columns = this.Options.Properties.Select(args => $"{ColumnPrefix}{args}")
			.Union(new string[] {
					nameof(ChangeReport<object>.Property),
					nameof(ChangeReport<object>.OriginalValue),
					nameof(ChangeReport<object>.CurrentValue)
			}).ToArray();
			var option = new PrintTableOption(columns) {
				GetColumnHeader = args => (args.StartsWith(ColumnPrefix) ? args.Substring(ColumnPrefix.Length) : args).Replace(".", ""),
				FormatValue = Options.FormatValueFunc ?? PrintOption.DefaultFormatValue,
			};
			await writer.PrintTable(this.Changes.ToArray(), option);
			if (options.Postfix != null) {
				writer.Append(options.Postfix);
			}
			return writer.ToString();
		}

		public async Task PostSave() {
			if (this.Changes.Any()) {
				this.Text = await BuildText(this.Options, this.Changes);
				if(SendReport!= null) {
					await SendReport(this.Text);
				}
			}
		}
	}
}
