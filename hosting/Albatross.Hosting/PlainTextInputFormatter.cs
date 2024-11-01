using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.DependencyInjection {
	public class PlainTextInputFormatter : TextInputFormatter {
		public PlainTextInputFormatter() {
			SupportedMediaTypes.Add("text/plain");
			SupportedMediaTypes.Add("text/html");
			SupportedEncodings.Add(UTF8EncodingWithoutBOM);
			SupportedEncodings.Add(UTF16EncodingLittleEndian);
		}

		protected override bool CanReadType(Type type) {
			return type == typeof(string);
		}

		public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding) {
			string data = null;
			using (var streamReader = context.ReaderFactory(context.HttpContext.Request.Body, encoding)) {
				data = await streamReader.ReadToEndAsync();
			}
			return InputFormatterResult.Success(data);
		}
	}
}