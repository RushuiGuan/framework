using Microsoft.Extensions.Logging;
using System;

namespace Albatross.Logging {

	/// <summary>
	/// Use CustomLogger class to create a Logger<typeparamref name="T"/> with a custom logger name.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CustomLogger<T> : ILogger<T> {
		private readonly ILogger logger;
		public CustomLogger(ILoggerFactory factory, IGetLoggerName getLoggerName) {
			logger = factory.CreateLogger(getLoggerName.Get(typeof(T)));
		}
		public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>this.logger.BeginScope(state);
		public bool IsEnabled(LogLevel logLevel) => this.logger.IsEnabled(logLevel);
		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) 
			=> this.logger.Log(logLevel, eventId, state, exception, formatter);
	}
}
