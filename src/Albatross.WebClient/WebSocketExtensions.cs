using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.WebClient {
	public static class WebSocketExtensions {
		public const int SendBufferSize = 1024 * 8;
		public const int ReceiveBufferSize = 1024 * 8;

		public static async Task<WebSocketMessageType> Receive(this WebSocket socket, Memory<byte> buffer, Stream stream, CancellationToken cancellation) {
			ValueWebSocketReceiveResult result;
			do {
				result = await socket.ReceiveAsync(buffer, cancellation);
				ReadOnlyMemory<byte> @readonly;
				if (result.Count < buffer.Length) {
					@readonly = buffer.Slice(0, result.Count);
				} else {
					@readonly = buffer;
				}
				await stream.WriteAsync(@readonly, cancellation);
			} while (!result.EndOfMessage);
			return result.MessageType;
		}

		public static async Task Send(this WebSocket socket, byte[] data, WebSocketMessageType messageType, CancellationToken cancellation, int bufferSize = SendBufferSize) {
			var buffer = new Memory<byte>(data);
			int left = data.Length, offset = 0;
			do {
				int count = System.Math.Min(bufferSize, left);
				left -= count;
				await socket.SendAsync(buffer.Slice(offset, count), messageType, left == 0, cancellation);
				offset += count;
			} while (left > 0);
		}

		public static async Task<T?> Receive<T>(this WebSocket socket, CancellationToken cancellation, JsonSerializerOptions serializerOptions, ILogger logger, int bufferSize = ReceiveBufferSize) {
			Memory<byte> buffer = new Memory<byte>(new byte[bufferSize]);
			using var stream = new MemoryStream();
			var type = await socket.Receive(buffer, stream, cancellation);
			if (type == WebSocketMessageType.Close) {
				throw new WebSocketException(WebSocketError.ConnectionClosedPrematurely, "Connection closed during a read operation");
			} else {
				var array = stream.ToArray();
				string text = UTF8Encoding.UTF8.GetString(array);
				logger.LogInformation("socket-r:{msg}", text);

				var result = JsonSerializer.Deserialize<T>(new ReadOnlySpan<byte>(array), serializerOptions);
				return result;
			}
		}
		public static async Task Send<T>(this WebSocket socket, T t, CancellationToken cancellation, JsonSerializerOptions serializerOptions, Microsoft.Extensions.Logging.ILogger logger) {
			var data = JsonSerializer.SerializeToUtf8Bytes<T>(t, serializerOptions);
			await socket.Send(data, WebSocketMessageType.Text, cancellation);

			string text = UTF8Encoding.UTF8.GetString(data);
			logger.LogInformation("socket-w: {msg}", text);
		}
	}
}