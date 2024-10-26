namespace Albatross.CodeGen.WebClient.Settings {
	public record class WebClientMethodSettings {
		public bool? UseDateTimeAsDateOnly { get; set; }

		public WebClientMethodSettings Overwrite(WebClientMethodSettings other) {
			return new WebClientMethodSettings {
				UseDateTimeAsDateOnly = UseDateTimeAsDateOnly ?? other.UseDateTimeAsDateOnly
			};
		}
	}
}