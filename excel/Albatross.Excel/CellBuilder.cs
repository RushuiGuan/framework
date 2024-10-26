using Albatross.Collections;
using ExcelDna.Integration;
using System;
using System.Collections.Generic;

namespace Albatross.Excel {
	public class CellBuilder {
		ExcelReference range;
		Dictionary<Type, CellProperty> properties = new Dictionary<Type, CellProperty>();

		bool selected = false;
		public CellBuilder(ExcelReference range) {
			this.range = range;
		}
		public CellBuilder(int rowFirst, int columnIndex) {
			this.range = new ExcelReference(rowFirst, columnIndex);
		}

		public CellBuilder(int rowFirst, int rowLast, int columnFirst, int columnLast, string? sheetName = null) {
			if (string.IsNullOrEmpty(sheetName)) {
				this.range = new ExcelReference(rowFirst, rowLast, columnFirst, columnLast);
			} else {
				this.range = new ExcelReference(rowFirst, rowLast, columnFirst, columnLast, sheetName);
			}
		}

		public CellBuilder SetValue(object value) {
			this.range.SetValue(value);
			return this;
		}
		public CellBuilder Use<T>(T cellProperty) where T : CellProperty {
			properties[typeof(T)] = cellProperty;
			return this;
		}
		public CellBuilder Use<T>(Action<T> action) where T : CellProperty, new() {
			var cellProperty = properties.GetOrAdd(typeof(T), () => new T());
			action((T)cellProperty);
			return this;
		}
		public void Focus() {
			if (!this.selected) {
				this.selected = true;
			}
		}
		public void Apply() {
			foreach (var item in this.properties.Values) {
				item.Apply(this.range);
			}
		}
	}
}