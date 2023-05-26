using Microsoft.Extensions.Logging;
using System;

namespace Albatross.Logging {
	public class LoggerEx<T> : ILogger<T> {
		private readonly ILogger logger;

		public LoggerEx(ILoggerFactory factory) {
			if (factory == null) {
				throw new ArgumentNullException(nameof(factory));
			}
			logger = factory.CreateLogger(typeof(T).Name);
		}

		public IDisposable? BeginScope<TState>(TState state) where TState : notnull => logger.BeginScope(state);
		
		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) 
			=> logger.Log(logLevel, eventId, state, exception, formatter);

		bool ILogger.IsEnabled(LogLevel logLevel) => logger.IsEnabled(logLevel);
	}
}
