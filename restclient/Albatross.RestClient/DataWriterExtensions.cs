using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Albatross.RestClient {
	public static class DataWriterExtensions {
		public static async Task LogResponse(this TextWriter? writer, HttpResponseMessage response, bool logContent = true) {
			if (writer != null) {
				writer.WriteLine("-------------------- Response --------------------");
				writer.WriteLine($"status-code: {response.StatusCode}({(int)response.StatusCode})");
				LogHeader(writer, response.Headers);
				LogHeader(writer, response.Content.Headers);
				if (logContent) {
					writer.WriteLine("-------------------- Content --------------------");
					var content = await response.ReadResponseAsText();
					if (!content.EndsWith('\n')) {
						writer.WriteLine(content);
					} else {
						writer.Write(content);
					}
				}
			}
		}
		static void LogHeader(TextWriter myWriter, HttpHeaders headers) {
			foreach (var header in headers) {
				myWriter.Write(header.Key);
				myWriter.Write(":");
				foreach (var value in header.Value) {
					myWriter.Write(" ");
					myWriter.Write(value);
				}
				myWriter.WriteLine();
			}
		}
		public static void LogRequest(this TextWriter? writer, HttpRequestMessage request) {
			if (writer != null) {
				writer.WriteLine("-------------------- Request --------------------");
				writer.Write(request.Method);
				writer.Write(" ");
				writer.WriteLine(request.RequestUri);
				writer.WriteLine("-------------------- Headers --------------------");
				LogHeader(writer, request.Headers);
				if (request.Content != null) {
					LogHeader(writer, request.Content.Headers);
					writer.WriteLine("-------------------- Content --------------------");
					string text = request.Content.ReadAsStringAsync().Result;
					writer.WriteLine(text);
				}
				writer.WriteLine();
			}
		}
	}
}