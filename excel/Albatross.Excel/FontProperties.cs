using ExcelDna.Integration;

namespace Albatross.Excel {
	public abstract record class CellProperty {
		public abstract void Apply(ExcelReference range);
	}
	public record class FontProperties : CellProperty{
		object font = ExcelMissing.Value;
		object fontStyle = ExcelMissing.Value;
		object color = ExcelMissing.Value;
		object size = ExcelMissing.Value;
		object strikethrough = ExcelMissing.Value;
		object superscript = ExcelMissing.Value;
		object subscript = ExcelMissing.Value;
		object underline = ExcelMissing.Value;

		public bool IsChanged => this != new FontProperties();
		public override void Apply(ExcelReference range) {
			if (this.IsChanged) {
				XlCall.Excel(XlCall.xlcSelect, range);
				XlCall.Excel(XlCall.xlcFontProperties,
					this.font,
					this.fontStyle,
					this.size,
					this.strikethrough,
					this.superscript,
					this.subscript,
					ExcelMissing.Value,
					ExcelMissing.Value,
					this.underline,
					this.color);
			}
		}
		public FontProperties Font(string font) {
			this.font = font;
			return this;
		}
		public FontProperties Bold() {
			this.fontStyle = "Bold";
			return this;
		}
		public FontProperties Italic() {
			this.fontStyle = "Italic";
			return this;
		}
		public FontProperties BoldItalic() {
			this.fontStyle = "Bold Italic";
			return this;
		}
		public FontProperties Regular() {
			this.fontStyle = "Regular";
			return this;
		}
		public FontProperties Color(Color color) {
			this.color = (int)color;
			return this;
		}
		public FontProperties Size(int size) {
			this.size = size;
			return this;
		}
		public FontProperties Superscript(bool set = true) {
			superscript = set;
			if (set) {
				subscript = ExcelMissing.Value;
			}
			return this;
		}
		public FontProperties Strikethrough(bool set = true) {
			strikethrough = set;
			return this;
		}
		public FontProperties Subscript(bool set = true) {
			subscript = set;
			if (set) {
				superscript = ExcelMissing.Value;
			}
			return this;
		}
		public FontProperties NoUnderline() {
			underline = 1;
			return this;
		}
		public FontProperties SingleUnderline() {
			underline = 2;
			return this;
		}
		public FontProperties DoubleUnderline() {
			underline = 3;
			return this;
		}
		public FontProperties SingleAccountingUnderline() {
			underline = 4;
			return this;
		}
		public FontProperties DoubleAccountingUnderline() {
			underline = 5;
			return this;
		}
	}
}
