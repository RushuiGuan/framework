#if NET8_0_OR_GREATER
using MessagePack;
namespace Albatross.IO {
	[MessagePackObject]
	public record class FileIndexValue<KeyType> {
		public FileIndexValue(KeyType key, long position) {
			Key = key;
			Position = position;
		}
		
		[Key(0)]
		public KeyType Key { get; }

		[Key(1)]
		public long Position { get; }
	}
}
#endif
