namespace Albatross.DependencyInjection {
	public class ErrorMessage {
		public string? Message { get; set; }
		public string? Type { get; set; }
		public int StatusCode { get; set; }

		public ErrorMessage? InnerError { get; set; }
	}
}