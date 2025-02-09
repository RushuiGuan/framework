namespace Albatross.Text.Table {
	public class StringTable {
		public class Column {
			public string Name { get; }
			public int MaxWidth { get; set; }
			public Column(string name) {
				Name = name;
				MaxWidth = name.Length;
			}
			public bool AlignRight { get; set; }
		}
		public class Row {
			public string[] Values { get; }
			public Row(params string[] values) {
				Values = values;
			}
		}
		Column[] columns;
		List<Row> rows = new List<Row>();

		public Column[] Columns => columns;
		public IEnumerable<Row> Rows => rows;

		public StringTable(params IEnumerable<string> headers) {
			columns = headers.Select(x => new Column(x)).ToArray();
		}
		public void Add(params IEnumerable<string> values) {
			var array = values.ToArray();
			if (array.Length != columns.Length) {
				throw new ArgumentException($"Table is expecting rows with {columns.Length} columns");
			}
			rows.Add(new Row(array));
			for (int i = 0; i < columns.Length; i++) {
				if (columns[i].MaxWidth < array[i].Length) {
					columns[i].MaxWidth = array[i].Length;
				}
			}
		}
	}
}
