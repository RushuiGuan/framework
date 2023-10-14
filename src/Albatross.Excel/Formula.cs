using Albatross.Excel.Table;
using ExcelDna.Integration;

namespace Albatross.Excel {
	public record class Formula : CellProperty {
		public string? Value { get; set; }
		public bool HasFormula => !string.IsNullOrEmpty(Value);
		public override void Apply(ExcelReference range) {
			if (HasFormula) {
				if (range.IsSingleCell()) {
					XlCall.Excel(XlCall.xlcFormula, this.Value, range);
				} else {
					XlCall.Excel(XlCall.xlcFormulaFill, this.Value, range);
				}
			}
		}
	}
}
