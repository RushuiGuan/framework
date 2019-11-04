using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.Host.AspNetCore {
	public class GlobalExceptionHandler  {
		public async Task RunAsync(HttpContext context) {
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			context.Response.ContentType = "application/json";
			Exception error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
			if (error != null) {
				var msg = new {
					Message = "A server error has occurred.",
					ExceptionType = error.GetType().FullName,
					StackTrace = error.StackTrace,
					ExceptionMessage = error.Message,
				};

				using (var writer = new Utf8JsonWriter(context.Response.Body)) {
					JsonSerializer.Serialize(writer, msg);
					await writer.FlushAsync().ConfigureAwait(false);
				}
			}
		}
	}
}