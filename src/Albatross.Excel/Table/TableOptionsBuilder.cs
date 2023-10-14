using Albatross.Collections;
using Albatross.Excel;
using ExcelDna.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Albatross.Excel.Table {
	public class TableOptionsBuilder {
		TableOptions options = new TableOptions();
		TableColumn? current;
		List<Action<TableColumn>> templates = new List<Action<TableColumn>>();
		Dictionary<string, TableColumn> columns = new Dictionary<string, TableColumn>();
		public TableColumn Current => this.current ?? throw new InvalidOperationException("Current table column is not set");
		public IEnumerable<TableColumn> Results => this.columns.Values;


		#region templates
		public TableOptionsBuilder AddTemplate(Action<TableColumn> action) {
			templates.Add(action);
			return this;
		}
		public TableOptionsBuilder ClearTemplate() {
			templates.Clear();
			return this;
		}
		public TableOptionsBuilder ApplyTemplate() {
			foreach (var template in this.templates) {
				template(this.Current);
			}
			return this;
		}
		public TableOptionsBuilder ApplyTemplate(params string[] names) {
			foreach (var name in names) {
				this.SetCurrent(name).ApplyTemplate();
			}
			return this;
		}
		#endregion

		#region cell properties
		public TableOptionsBuilder SetFormula(string formula) {
			Current.Formula.Value = formula;
			return this;
		}
		public TableOptionsBuilder SetBackground(Action<Background> action) {
			action(Current.Background);
			return this;
		}
		public TableOptionsBuilder SetNumberFormat(Action<NumberFormat> action) {
			action(Current.NumberFormat);
			return this;
		}
		public TableOptionsBuilder SetFontProperties(Action<FontProperties> action) {
			action(Current.FontProperties);
			return this;
		}
		#endregion

		#region add, remove and set current column
		public TableOptionsBuilder SetCurrent(string name, Action<TableColumn>? changes = null) {
			if (this.columns.TryGetValue(name, out var value)) {
				this.current = value;
				if (changes != null) { changes(value); }
				return this;
			} else {
				throw new ArgumentException($"Column {name} is not an existing column");
			}
		}
		public TableOptionsBuilder Add(string name, Type type, string? title = null) => this.Add(new TableColumn(name, type, title));
		public TableOptionsBuilder Add(TableColumn item) {
			if (columns.ContainsKey(item.Name)) {
				throw new ArgumentException($"{item.Name} is the name of an existing column.  Column names should be unique.");
			}
			columns.Add(item.Name, item);
			item.Order = columns.Count;
			this.current = item;
			this.ApplyTemplate();
			return this;
		}
		public TableOptionsBuilder AddColumns<T>() {
			foreach (var item in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
				.Select(x => new { PropertyInfo = x, Attribute = x.GetCustomAttribute<ExcelColumnAttribute>() })
				.Where(x => x.Attribute?.Hidden != true)
				.Select(x => new TableColumn(x.PropertyInfo, x.Attribute))) {
				this.Add(item);
			}
			return this;
		}
		public TableOptionsBuilder Remove(string name) {
			if (columns.TryGetAndRemove(name, out var _)) {
				if (current?.Name == name) {
					current = null;
				}
			}
			return this;
		}
		#endregion

		#region attributes
		public TableOptionsBuilder Set(Action<TableColumn> action) {
			action(this.Current);
			return this;
		}
		public TableOptionsBuilder Order(float order) {
			this.Current.Order = order;
			return this;
		}
		public TableOptionsBuilder ColumnType<T>() {
			this.Current.Type = typeof(T);
			return this;
		}
		public TableOptionsBuilder UseReadPropertyHandler(ReadEntityPropertyDelegate getCellValueHandler) {
			Current.ReadEntityPropertyHandler = getCellValueHandler;
			return this;
		}
		public TableOptionsBuilder UseWritePropertyHandler(TrySetEntityPropertyDelegate trySetEntityPropertyHandler) {
			Current.ReadOnly = false;
			Current.TrySetEntityPropertyHandler = trySetEntityPropertyHandler;
			return this;
		}
		public TableOptionsBuilder ReadOnly(bool readOnly = true) {
			Current.ReadOnly = readOnly;
			return this;
		}
		public TableOptionsBuilder KeyColumn(string name) {
			if (columns.TryGetValue(name, out var value)) {
				value.IsKey = true;
				return this;
			} else {
				throw new ArgumentException($"{name} is not a valid column name");
			}
		}
		public TableOptionsBuilder Offset(int rowOffset, int columnOffset) {
			options.FirstRow = rowOffset;
			options.FirstColumn = columnOffset;
			return this;
		}
		#endregion

		public void UpdateFormula(TableColumn column) {
			var sb = new StringBuilder(column.Formula.Value);
			foreach(var item in this.columns.Values) {
				sb.Replace("<" + item.Name + ">", string.Format("RC[{0}]", item.Index - column.Index));
			}
			column.Formula.Value = sb.ToString();
		}

		public TableOptions Build() {
			// create the columns in one pass
			var hasKey = false;
			var array = columns.Values.OrderBy(args=>args.Order).ThenBy(args=>args.Name).ToArray();
			options.Columns = new TableColumn[array.Length];
			var formulaColumns = new List<TableColumn>();
			for (int i = 0; i < array.Length; i++) {
				var column = array[i];
				if (column.IsKey) { hasKey = true; }
				column.Index = i;
				column.AutoFormat();
				options.Columns[i] = column;
				if (column.Formula.HasFormula) {
					formulaColumns.Add(column);
				}
			}
			// second pass to update formula
			foreach(var column in formulaColumns) {
				UpdateFormula(column);
			}
			if (!hasKey) {
				options.Columns[0].IsKey = true;
			}
			return options;
		}
	}
}