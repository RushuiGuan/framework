using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Albatross.Text.Table {
	public class TableColumnOption<T> {
		public required Func<T, object?> GetValueDelegate { get; init; }
		public required Func<T, object?, string> Formatter { get; init; }
		public required string Header { get; init; }
		public required int Order { get; set; }
		public required string Property { get; set; }
	}
	public abstract class TableOptions {
		public abstract Type Type { get; }
	}
	public class TableOptions<T> : TableOptions {
		public override Type Type => typeof(T);
		public ImmutableArray<TableColumnOption<T>> ColumnOptions { get; }

		public TableOptions() { }
		public TableOptions(TableOptionBuilder<T> builder) {
			var list = new List<TableColumnOption<T>>();
			foreach (var keyValue in builder.ColumnOptionBuilders) {
				list.Add(new TableColumnOption<T> {
					GetValueDelegate = keyValue.Value.GetValueDelegate,
					Formatter = keyValue.Value.Formatter,
					Header = keyValue.Value.GetHeader(),
					Order = keyValue.Value.GetOrder(),
					Property = keyValue.Key,
				});
			}
			this.ColumnOptions = list.OrderBy(x => x.Order)
				.ThenBy(x => x.Header)
				.ThenBy(x => x.Property)
				.ToImmutableArray();
		}

		public IEnumerable<string> GetValue(T item) {
			for (int i = 0; i < ColumnOptions.Length; i++) {
				var value = ColumnOptions[i].GetValueDelegate(item);
				yield return ColumnOptions[i].Formatter(item, value);
			}
		}
	}
}
