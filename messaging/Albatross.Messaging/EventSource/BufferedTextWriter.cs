using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;

namespace Albatross.Messaging.EventSource {
	/// <summary>
	/// This text writer buffers text on a string builder object.  The buffer is reset
	/// when the <see cref="Begin"/> method is called.  The bufferred text will be returned when
	/// the <see cref="End"/> method is called.  During the <see cref="End"/> method call,
	/// a info log entry of the buffered text with the source context of "message-entry" will be created.  Special log configuration
	/// should be created to format the message-entry lines differently from the normal logs.
	/// This class is useful since it allows the caller to control the buffer mechanism.  In our situation, we want to buffer the write
	/// line by line.  
	/// </summary>
	public class BufferedTextWriter {
		StringBuilder stringBuilder;
		StringWriter writer;
		ILogger logger;

		public BufferedTextWriter(string name, ILoggerFactory loggerFactory) {
			stringBuilder = new StringBuilder();
			writer = new StringWriter(stringBuilder);
			this.logger = loggerFactory.CreateLogger($"{name}-message-entry");
		}
		public TextWriter Begin() {
			this.stringBuilder.Length = 0;
			return this.writer;
		}
		public string End() {
			var line = stringBuilder.ToString();
			logger.LogInformation(line);
			return line;
		}
	}
}