using ExcelDna.Integration;

namespace Albatross.Excel {
	public record class Background : CellProperty {
		object color = ExcelMissing.Value;
		object pattern = ExcelMissing.Value;

		public Background Pattern(Pattern? pattern) {
			if (pattern.HasValue) {
				this.pattern = (int)pattern;
			}
			return this;
		}
		public Background Solid() {
			return this.Pattern(Excel.Pattern.Solid);
		}
		public Background Color(Color? color) {
			if (color.HasValue) {
				this.color = (int)color;
			}
			return this;
		}
		public override void Apply(ExcelReference range) {
			if (color != ExcelMissing.Value) {
				if (pattern == ExcelMissing.Value) { this.Solid(); }
				XlCall.Excel(XlCall.xlcSelect, range);
				XlCall.Excel(XlCall.xlcPatterns, pattern, this.color, ExcelMissing.Value);
			}
		}
	}
}