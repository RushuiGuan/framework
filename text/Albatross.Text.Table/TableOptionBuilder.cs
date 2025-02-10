using System;
using System.Collections.Generic;

namespace Albatross.Text.Table {
	public class TableColumnOptionBuilder<T> {
		public required Func<T, object?> GetValueDelegate { get; set; }
		public required Func<T, object?, string> Formatter { get; set; }
		public required Func<string> GetHeader { get; set; }
		public required Func<int> GetOrder { get; set; }
	}

	public class TableOptionBuilder<T> {
		public Dictionary<string, TableColumnOptionBuilder<T>> ColumnOptionBuilders { get; } = new Dictionary<string, TableColumnOptionBuilder<T>>();

		public TableOptionBuilder<T> ColumnOrder(string property, Func<int> getOrder) {
			this.ColumnOptionBuilders[property].GetOrder = getOrder;
			return this;
		}

		public TableOptionBuilder<T> Exclude(string property) {
			this.ColumnOptionBuilders.Remove(property);
			return this;
		}

		public TableOptionBuilder<T> SetColumn(string column, Func<T, object?> getValue) {
			if (this.ColumnOptionBuilders.TryGetValue(column, out var columnOptionBuilder)) {
				columnOptionBuilder.GetValueDelegate = getValue;
			} else {
				var count = this.ColumnOptionBuilders.Count;
				ColumnOptionBuilders[column] = new TableColumnOptionBuilder<T> {
					GetValueDelegate = getValue,
					Formatter = (T entity, object? value) => BuilderExtensions.DefaultFormat(value),
					GetHeader = () => column,
					GetOrder = () => count,
				};
			}
			return this;
		}

		public TableOptionBuilder<T> ColumnHeader(string property, Func<string> getColumnHeader) {
			this.ColumnOptionBuilders[property].GetHeader = getColumnHeader;
			return this;
		}

		public TableOptionBuilder<T> Format(string property, Func<T, object?, string> format) {
			this.ColumnOptionBuilders[property].Formatter = format;
			return this;
		}
	}
}
