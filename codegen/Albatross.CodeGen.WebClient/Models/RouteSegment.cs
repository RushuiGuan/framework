using Albatross.CodeAnalysis.Symbols;

namespace Albatross.CodeGen.WebClient.Models {
	public interface IRouteSegment { }
	public record class RouteTextSegment : IRouteSegment {
		public RouteTextSegment(string text) {
			Text = text;
		}
		public string Text { get; }
		public override string ToString() => Text;
	}
	public record class RouteParameterSegment : IRouteSegment {
		public RouteParameterSegment(string text) {
			this.Text = text;
		}
		public ParameterInfo? ParameterInfo { get; set; }
		public string Text { get; init; }

		public override string ToString() {
			var type = ParameterInfo?.Type.GetFullName();
			if (type == "System.DateTime" || type == "System.DateTimeOffset" || type == "System.DateOnly" || type == "System.TimeOnly") {
				return $"{{{Text}.ISO8601String()}}";
			} else {
				return $"{{{Text}}}";
			}
		}
	}
}
