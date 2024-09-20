using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Albatross.IO {
	public static class AsciiExtensions {
		public static bool TryReadLastLineFromStream(this Stream stream, [NotNullWhen(true)] out string? line, bool seekToEnd = true) {
			if (stream.CanSeek) {
				if (seekToEnd) { stream.Seek(0, SeekOrigin.End); }
				var position = stream.Position;
				Stack<char> stack = new Stack<char>();
				while (position > 0) {
					position--;
					stream.Seek(position, SeekOrigin.Begin);
					var @byte = stream.ReadByte();
					if (@byte == '\n' && stack.Count > 0) {
						break;
					} else if (@byte != '\r' && @byte != '\n') {
						stack.Push((char)@byte);
					}
				}
				if (stack.Count > 0) {
					stream.Seek(position, SeekOrigin.Begin);
					line = new string(stack.ToArray());
					return true;
				}
			}
			line = null;
			return false;
		}

		public static bool TryReadLineFromStream(this Stream stream, [NotNullWhen(true)] out string? line) {
			var sb = new StringBuilder(128);
			if (stream.TryReadLineFromStream(sb)) {
				line = sb.ToString();
				return true;
			} else {
				line = null;
				return false;
			}
		}

		public static bool TryReadLineFromStream(this Stream stream, StringBuilder sb) {
			var before = sb.Length;
			while (stream.Position < stream.Length) {
				var @byte = stream.ReadByte();
				if (@byte == '\n' && sb.Length > 0) {
					break;
				} else if (@byte != '\r' && @byte != '\n') {
					sb.Append((char)@byte);
				}
			}
			return sb.Length > before;
		}
	}
}
