using Albatross.Cryptography.Core;
using System.Security.Cryptography;
using System.Text;

namespace Albatross.Cryptography {
	/// <summary>
	/// Create a 32 byte, 256 bit hash
	/// </summary>
	public class CreateSHA256Hash : ICreateHash {
		public int HashSize => 32;

		public byte[] Create(string signature) {
			byte[] bytes = Encoding.UTF8.GetBytes(signature);
			using (var sha = SHA256.Create()) {
				return sha.ComputeHash(bytes);
			}
		}
	}
}
