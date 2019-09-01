using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Host.AspNetCore {
	public class GlobalExceptionHandler : IMiddleware {
		JsonSerializer serializer;

		public GlobalExceptionHandler(IGetServerJsonSerializer getJsonSerializer) {
			this.serializer = getJsonSerializer.Get();
		}
		public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
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
				using (var writer = new StreamWriter(context.Response.Body)) {
					serializer.Serialize(writer, msg);
					await writer.FlushAsync().ConfigureAwait(false);
				}
			}
		}
	}
}