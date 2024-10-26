using Albatross.Text;
using NetMQ;
using System;

namespace Albatross.Messaging.Messages {
	public static class Extensions {
		/// <summary>
		/// Since the routing frame is always followed by an empty frame.  If the routing frame has been removed by the (dealer) socket, the first frame
		/// should always be empty.  
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static bool HasRoute(this NetMQMessage message) => message.FrameCount == 0 ? false : message[0].BufferSize > 0;

		public static string PeekMessageHeader(this NetMQMessage msg) {
			var headerIndex = msg.HasRoute() ? 2 : 1;
			if (headerIndex >= msg.FrameCount) {
				throw new MissingMessageFrameException(msg.FrameCount, headerIndex + 1);
			}
			return msg[headerIndex].Buffer.ToUtf8String();
		}
		public static string PeekMessageHeader(this string line, int offset) {
			if (line.TryGetText(Message.LogDelimiter, ref offset, out var header)) {
				return header;
			} else {
				throw new InvalidMsgLogException(line);
			}
		}
		public static NetMQMessage Create(this IMessage message) {
			var frames = new NetMQMessage(7);
			message.WriteToFrames(frames);
			return frames;
		}
	}
}