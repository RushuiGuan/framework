using Albatross.Cryptography.Core;
using System.Security.Cryptography;
using System.Text;

namespace Albatross.Cryptography {
	/// <summary>
	/// Create a 64 byte, 512 bit hash
	/// </summary>
	public class CreateSHA512Hash : ICreateHash{
		public int HashSize => 64;

		public byte[] Create(string signature) {
			byte[] bytes = Encoding.UTF8.GetBytes(signature);
			using (var sha = SHA512.Create()) {
				return sha.ComputeHash(bytes);
			}
		}
	}
}
