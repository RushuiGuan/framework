using System.Collections.Immutable;

namespace Albatross.Text.Table {
	public abstract class TableOptions {
		public abstract Type Type { get; }
	}
	public class TableOptions<T> : TableOptions {
		public override Type Type => typeof(T);

		public ImmutableArray<Func<T, object?>> GetValueDelegates { get; }
		public ImmutableArray<Func<T, object?, string>> Formatters { get; }
		public ImmutableArray<string> ColumnHeaders { get; }


		public TableOptions(TableBuilder<T> builder) {
			var getValueDelegates = new List<Func<T, object?>>();
			var formatters = new List<Func<T, object?, string>>();
			var columnHeaders = new List<string>();
			foreach (var column in builder.ColumnOrders.OrderBy(x => x.Value).ThenBy(x => x.Key).Select(x => x.Key)) {
				getValueDelegates.Add(builder.GetValueDelegates[column]);
				formatters.Add(builder.Formatters[column]);
				columnHeaders.Add(builder.ColumnHeaders[column]);
			}
			this.GetValueDelegates = getValueDelegates.ToImmutableArray();
			this.Formatters = formatters.ToImmutableArray();
			this.ColumnHeaders = columnHeaders.ToImmutableArray();
		}

		public IEnumerable<string> GetValue(T item) {
			for(int i=0; i < GetValueDelegates.Length; i++) {
				var value = GetValueDelegates[i](item);
				yield return Formatters[i](item, value);
			}
		}
	}
}
