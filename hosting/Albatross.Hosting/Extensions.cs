using System;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Albatross.Hosting {
	public static class Extensions {
		public static void SetCurrentDirectory() {
			System.Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(typeof(Extensions).Assembly.Location)!;
		}

		public static ErrorMessage ErrorMessage(this Exception err, ILogger logger, [CallerMemberName] string methodName = "") {
			logger.LogWarning(err, methodName);
			return new ErrorMessage {
				Message = err.Message,
				Type = err.GetType().FullName,
			};
		}

		public static string ErrorTextMessage(this Exception err, ILogger logger, [CallerMemberName] string methodName = "") {
			logger.LogWarning(err, methodName);
			return err.Message;
		}
	}
}
