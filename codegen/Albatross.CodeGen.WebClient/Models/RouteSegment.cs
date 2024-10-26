namespace Albatross.CodeGen.WebClient.Models {
	public interface IRouteSegment {
		string Text { get; }
	}
	public record class RouteTextSegment : IRouteSegment {
		public RouteTextSegment(string text) {
			this.Text = text;
		}
		public string Text { get; }
	}
	public record class RouteParameterSegment : IRouteSegment {
		public RouteParameterSegment(string text) {
			this.Text = text;
		}

		public ParameterInfo? ParameterInfo { get; set; }
		public ParameterInfo RequiredParameterInfo => ParameterInfo ?? throw new System.InvalidOperationException("ParameterInfo is not set");
		public string Text { get; }
	}
}