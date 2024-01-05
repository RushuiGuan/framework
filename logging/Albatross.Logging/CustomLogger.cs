using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Albatross.Logging {
	public interface IGetLoggerName {
		string Get(Type type);
	}
	public class GetLoggerNameByNamespacePrefixExclusion : IGetLoggerName {
		private readonly string[] excludeNamespaces;
		public GetLoggerNameByNamespacePrefixExclusion(string[] excludeNamespaces) {
			this.excludeNamespaces = excludeNamespaces;
		}
		public string Get(Type type) {
			if (excludeNamespaces.Any(x => type.FullName.StartsWith(x))) {
				return type.FullName;
			} else {
				return type.Name;
			}
		}
	}	
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
