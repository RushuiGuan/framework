using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;
using System;
using System.Text;

namespace Albatross.Messaging {
	public static class Extensions {
		public static void ClientAck(this IMessagingService svc, string route, ulong id) => svc.Transmit(new ClientAck(route, id));
		public static void ServerAck(this IMessagingService svc, string route, ulong id) => svc.Transmit(new ServerAck(route, id));

		public static byte[] ToUtf8Bytes(this string text) => Encoding.UTF8.GetBytes(text);

		public static string ToUtf8String(this byte[] data) => Encoding.UTF8.GetString(data);
		public static double ToDouble(this byte[] data) => BitConverter.ToDouble(data);
		public static int ToInt(this byte[] data) => BitConverter.ToInt32(data);
		public static ulong ToULong(this byte[] data) => BitConverter.ToUInt64(data);
		public static long ToLong(this byte[] data) => BitConverter.ToInt64(data);
		public static bool ToBoolean(this byte[] data) => BitConverter.ToBoolean(data);

		public static string PopUtf8String(this NetMQMessage frames) => Encoding.UTF8.GetString(frames.Pop().Buffer);
		public static void AppendUtf8String(this NetMQMessage frames, string? text) {
			if (string.IsNullOrEmpty(text)) {
				frames.AppendEmptyFrame();
			} else {
				frames.Append(Encoding.UTF8.GetBytes(text));
			}
		}

		public static double PopDouble(this NetMQMessage frames) => BitConverter.ToDouble(frames.Pop().Buffer, 0);
		public static void AppendDouble(this NetMQMessage frames, double value)
			=> frames.Append(BitConverter.GetBytes(value));

		public static bool PopBoolean(this NetMQMessage frames) => BitConverter.ToBoolean(frames.Pop().Buffer, 0);
		public static void AppendBoolean(this NetMQMessage frames, bool value)
			=> frames.Append(BitConverter.GetBytes(value));

		public static int PopInt(this NetMQMessage frames) => BitConverter.ToInt32(frames.Pop().Buffer, 0);
		public static void AppendInt(this NetMQMessage frames, int value)
			=> frames.Append(BitConverter.GetBytes(value));

		public static uint PopUInt(this NetMQMessage frames) => BitConverter.ToUInt32(frames.Pop().Buffer, 0);
		public static void AppendUInt(this NetMQMessage frames, uint value)
			=> frames.Append(BitConverter.GetBytes(value));

		public static void AppendInt16(this NetMQMessage frames, short value)
			=> frames.Append(BitConverter.GetBytes(value));

		public static ulong PopULong(this NetMQMessage frames) => BitConverter.ToUInt64(frames.Pop().Buffer, 0);
		public static void AppendULong(this NetMQMessage frames, ulong value)
			=> frames.Append(BitConverter.GetBytes(value));

		public static long PopLong(this NetMQMessage frames) => BitConverter.ToInt64(frames.Pop().Buffer, 0);
		public static void AppendLong(this NetMQMessage frames, long value)
			=> frames.Append(BitConverter.GetBytes(value));
	}
}