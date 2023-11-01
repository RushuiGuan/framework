using Albatross.Reflection;
using ExcelDna.Integration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Albatross.Excel.Table {
	public delegate bool TrySetEntityPropertyDelegate(object entity, TableColumn column, object cellValue, out string? error);
	public delegate object? ReadEntityPropertyDelegate(TableColumn column, object entity);

	public record class TableColumn {
		public float Order { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public Type Type { get; set; }
		public bool IsNullable { get; set; }
		public bool Required { get; set; }
		public bool UseNullForError { get; set; }
		public bool ReadOnly { get; set; }
		public int Index { get; set; }
		public bool IsKey { get; set; }
		public FontProperties FontProperties { get; } = new FontProperties();
		public Background Background { get; } = new Background();
		public NumberFormat NumberFormat { get; } = new NumberFormat();
		public Formula Formula { get; set; } = new Formula();
		public ReadEntityPropertyDelegate ReadEntityPropertyHandler { get; set; } = (column, entity) => null;
		public TrySetEntityPropertyDelegate TrySetEntityPropertyHandler { get; set; }

		public TableColumn(PropertyInfo propertyInfo, ExcelColumnAttribute? attribute) {
			Name = propertyInfo.Name;
			Type = propertyInfo.PropertyType;
			Title = Name;
			TrySetEntityPropertyHandler = Extensions.TrySetEntityPropertyByReflection;
			ReadEntityPropertyHandler = (column, entity) => propertyInfo.GetValue(entity);
			ReadOnly = false;
			if (Type.IsValueType) {
				if(Type.GetNullableValueType(out var actualType)) {
					Type = actualType;
					IsNullable = true;
				}
			} else {
				IsNullable = new NullabilityInfoContext().Create(propertyInfo).WriteState == NullabilityState.Nullable;
			}
			if(attribute != null) {
				ReadOnly = attribute.ReadOnly;
				Required = attribute.Required;
				Title = attribute.Title ?? Title;
			}
		}
		public TableColumn(string name, Type type, string? title = null) {
			Name = name;
			Title = title ?? name;
			Type = type;
			TrySetEntityPropertyHandler = SetEntityPropertyNotSupported;
			ReadOnly = true;
		}

		public object? ReadEntityProperty(object entity) {
			var value = ReadEntityPropertyHandler(this, entity);
			if (value == null && (this.IsKey || Required)) {
				return ExcelError.ExcelErrorNull;
			} else {
				return CellValue.Write(value);
			}
		}
		public bool TrySetEntityProperty(object entity, object value, [NotNullWhen(false)] out string? error) {
			return TrySetEntityPropertyHandler(entity, this, value, out error);
		}
		public void AutoFormat() {
			if(!this.NumberFormat.HasValue) {
				if (Type == typeof(DateTime)) {
					this.NumberFormat.StandardDate();
				}
			}
		}
		public static bool SetEntityPropertyNotSupported(object entity, TableColumn column, object cellValue, out string? error) {
			error = "Not supported";
			return false;
		}
	}
}