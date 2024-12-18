using Albatross.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace Albatross.WebClient {
	// immutable class to hold the request options
	public record class RequestOptions {
		public TextWriter? DataWriter { get; init; }
		public ILogger Logger { get; init; } = NullLogger.Instance;
		public CancellationToken CancellationToken { get; init; } = CancellationToken.None;
		public JsonSerializerOptions SerializerOptions { get; init; } = DefaultJsonSettings.Value.Default;
	}
}