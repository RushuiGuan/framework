using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.WebClient.Models {
	public interface IRouteSegment { }
	public record class RouteTextSegmentInfo : IRouteSegment {
		public RouteTextSegmentInfo(string text) {
			Text = text;
		}
		public string Text { get; }
	}
	public record class RouteParameterSegmentInfo : IRouteSegment {
		public RouteParameterSegmentInfo(ParameterInfo parameterInfo) {
			ParameterInfo = parameterInfo;
		}
		public ParameterInfo ParameterInfo { get; }
		public override string ToString() {
			if (ParameterInfo.Type.SpecialType == SpecialType.System_DateTime) {
				return $"{{{ParameterInfo.Name}:yyyy-MM-dd}}";
			} else {
				return $"{{{ParameterInfo.Name}}}";
			}
		}
	}
}
