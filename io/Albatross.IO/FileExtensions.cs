using System.IO;
using System.Threading.Tasks;

namespace Albatross.IO {
	public static class FileExtensions {
		/// <summary>
		/// insert the content of the source stream to the destination stream.  If the copySource is true, the source stream 
		/// is copied to a temporary file before the prepend operation and therefore is untouched.  Otherwise the source stream is used 
		/// as the buffer file. The content of the source stream is not changed, however the last modified date of the source stream would be updated.
		/// Note that the positions of the source stream and the destination stream do not need to be at the beginning.  The resulting file will
		/// have the content of the source stream starting from its current position and the content of the destination stream starting from its current position.
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="source"></param>
		/// <returns></returns>
		public static async Task Prepend(this Stream destination, Stream source, bool copySource) {
			Stream? copy = null;
			try {
				long sourcePosition = source.Position;
				long destinationPosition = destination.Position;
				long totalLength = source.Length - source.Position + destination.Length - destination.Position;
				if (copySource) {
					copy = File.Open(Path.GetTempFileName(), FileMode.Create, FileAccess.ReadWrite);
					await source.CopyToAsync(copy);
					source = copy;
					sourcePosition = 0;
				}
				var srcLength = source.Length;
				// append the destination file to the end of the source file
				// the current position of the destination file might not be at the beginning.
				source.Seek(0, SeekOrigin.End);
				await destination.CopyToAsync(source);
				// then copy the source file to the destination file
				destination.Seek(0, SeekOrigin.Begin);
				source.Seek(sourcePosition, SeekOrigin.Begin);
				await source.CopyToAsync(destination);
				destination.SetLength(totalLength);
				// reset the length of the source file to its original length
				source.SetLength(srcLength);
			} finally {
				copy?.Dispose();
				if (copy != null) {
					File.Delete(((FileStream)copy).Name);
				}
			}
		}
		
		/// <summary>
		/// Append the remainng content of the second stream to the first stream.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static async Task Append(this Stream first, Stream second) {
			first.Seek(0, SeekOrigin.End);
			await second.CopyToAsync(first);
		}
		public static async Task<string> Append(this string firstFile, string secondFile) {
			using (var first = File.Open(firstFile, FileMode.Open, FileAccess.Write)) {
				using (var second = File.Open(secondFile, FileMode.Open, FileAccess.Read)) {
					await first.Append(second);
				}
			}
			return firstFile;
		}
	}
}
