using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.WebClient.CSharp;
using Albatross.CodeGen.WebClient.Settings;

namespace Albatross.CodeGen.WebClient.Models {
	public interface IRouteSegment {
		string Build(WebClientMethodSettings settings);
	}
	public record class RouteTextSegment : IRouteSegment {
		public RouteTextSegment(string text) {
			Text = text;
		}
		public string Text { get; }
		public string Build(WebClientMethodSettings settings) => Text;
	}
	public record class RouteParameterSegment : IRouteSegment {
		public RouteParameterSegment(string text) {
			this.Text = text;
		}
		public ParameterInfo? ParameterInfo { get; set; }
		public string Text { get; init; }

		public string Build(WebClientMethodSettings settings) {
			var type = ParameterInfo?.Type.GetFullName();
			if (type == "System.DateTime" && settings.UseDateTimeAsDateOnly == true) {
				return $"{{{Text}.ISO8601StringDateOnly()}}";
			} else if (type == "System.DateTime" || type == "System.DateTimeOffset" || type == "System.DateOnly" || type == "System.TimeOnly") {
				return $"{{{Text}.ISO8601String()}}";
			} else {
				return $"{{{Text}}}";
			}
		}
	}
}
