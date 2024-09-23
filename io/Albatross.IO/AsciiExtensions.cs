using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.IO {
	public static class AsciiExtensions {
		public static bool TryReadLastLine(this Stream stream, [NotNullWhen(true)] out string? line, bool seekToEnd = true) {
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

		public static bool TryReadLine(this Stream stream, [NotNullWhen(true)] out string? line, out long positionPrior) {
			var sb = new StringBuilder(128);
			if (stream.TryReadLine(sb, out positionPrior)) {
				line = sb.ToString();
				return true;
			} else {
				line = null;
				return false;
			}
		}

		public static bool TryReadLine(this Stream stream, StringBuilder sb, out long positionPrior) {
			positionPrior = stream.Position;
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

		public static async Task StitchChanges<T>(this string current, string changes, Func<string, T> getSortKey) where T : IComparable<T> {
			using (var stream = File.Open(current, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
				using (var changesStream = File.Open(changes, FileMode.Open, FileAccess.ReadWrite)) {
					if (changesStream.Length == 0) {
						return;
					}else if (stream.Length == 0) {
						await changesStream.CopyToAsync(stream);
					} else {
						T firstKey, lastKey, firstChangeKey, lastChangeKey;
						if (stream.TryReadLine(out var firstLine, out _) && stream.TryReadLastLine(out var lastLine)) {
							firstKey = getSortKey(firstLine);
							lastKey = getSortKey(lastLine);
							if (firstKey.CompareTo(lastKey) > 0) {
								throw new InvalidOperationException("Original file is not sorted");
							}
						} else {
							throw new InvalidOperationException("Original file is empty");
						}
						if (changesStream.TryReadLine(out firstLine, out _) && changesStream.TryReadLastLine(out lastLine)) {
							firstChangeKey = getSortKey(firstLine);
							lastChangeKey = getSortKey(lastLine);
							if (firstChangeKey.CompareTo(lastChangeKey) > 0) {
								throw new InvalidOperationException("Changes file is not sorted");
							}
						} else {
							throw new InvalidOperationException("Changes file is empty");
						}
						if (lastKey.CompareTo(firstChangeKey) < 0) {
							// changes after the current, exclusive the last key
							// if all of the changes are before the first key, simple prepend the changes to the current file
							stream.Seek(0, SeekOrigin.End);
							changesStream.Seek(0, SeekOrigin.Begin);
							await changesStream.CopyToAsync(stream);
						} else if (firstKey.CompareTo(lastChangeKey) > 0) {
							// changes before the current, exclusive the first key
							// if all of the changes are after the last key, simple append the changes to the current file
							stream.Seek(0, SeekOrigin.Begin);
							changesStream.Seek(0, SeekOrigin.Begin);
							await stream.Prepend(changesStream, false);
						} else if (firstChangeKey.CompareTo(firstKey) <= 0 && lastChangeKey.CompareTo(lastKey) >= 0) {
							// fully overlapped, changes completely covers the original file, inclusive of the first and last key
							// if the changed file completely covers the original file, then simply replace the original file with the changes
							stream.Seek(0, SeekOrigin.Begin);
							changesStream.Seek(0, SeekOrigin.Begin);
							await changesStream.CopyToAsync(stream);
							stream.SetLength(changesStream.Length);
						} else if (firstChangeKey.CompareTo(firstKey) > 0 && lastChangeKey.CompareTo(lastKey) < 0) {
							// overlapped within the original file, exclusive of the first and last key
							var alt_file = Path.GetTempFileName();
							using (var alt_stream = new FileInfo(alt_file).Open(FileMode.CreateNew, FileAccess.Write)) {
								long totalSize = changesStream.Length;
								stream.Seek(0, SeekOrigin.Begin);
								await stream.CopyToAsync(alt_stream);
								while (stream.TryReadLine(out var line, out var positionPrior)) {
									var key = getSortKey(line);
									if (key.CompareTo(firstChangeKey) >= 0) {
										totalSize += positionPrior + 1;
										alt_stream.Seek(positionPrior, SeekOrigin.Begin);
										await changesStream.CopyToAsync(alt_stream);
									}
									if (key.CompareTo(lastChangeKey) < 0) {
										stream.Seek(positionPrior, SeekOrigin.Begin);
										totalSize += stream.Length - positionPrior;
										break;
									}
								}
								await stream.CopyToAsync(alt_stream);
								alt_stream.SetLength(totalSize);
								alt_stream.Seek(0, SeekOrigin.Begin);
								stream.Seek(0, SeekOrigin.Begin);
								await alt_stream.CopyToAsync(stream);
								stream.SetLength(totalSize);
							}
							File.Delete(alt_file);
						} else if (lastChangeKey.CompareTo(firstKey) >= 0 && lastChangeKey.CompareTo(lastKey) < 0) {
							// overlapped before, inclusive of the first key, exclusive of the last key
							// if the changes start before the original file, but ends within the original file, find the point
							// where the changes end and prepend the changes to the original file at that position
							stream.Seek(0, SeekOrigin.Begin);
							while (stream.TryReadLine(out var line, out var positionPrior)) {
								var key = getSortKey(line);
								if (key.CompareTo(lastChangeKey) > 0) {
									stream.Seek(positionPrior, SeekOrigin.Begin);
									break;
								}
							}
							// reset the change stream so that we prepend the whole file to the current file
							changesStream.Seek(0, SeekOrigin.Begin);
							await stream.Prepend(changesStream, false);
						} else if (firstChangeKey.CompareTo(firstKey) > 0 && firstChangeKey.CompareTo(lastKey) <= 0) {
							// overlapped after, exclusive of the first key, inclusive of the last key
							stream.Seek(0, SeekOrigin.Begin);
							while (stream.TryReadLine(out var line, out var positionPrior)) {
								var key = getSortKey(line);
								if (key.CompareTo(firstChangeKey) >= 0) {
									stream.Seek(positionPrior, SeekOrigin.Begin);
									break;
								}
							}
							changesStream.Seek(0, SeekOrigin.Begin);
							await changesStream.CopyToAsync(stream);
						} else {
							throw new InvalidOperationException("Logic Error");
						}
					}
				}
			}
		}
	}
}
