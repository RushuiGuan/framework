using Albatross.Cryptography.Core;
using System;
using System.Security.Cryptography;

namespace Albatross.Cryptography {
	/// <summary>
	/// Create a 32 byte hash with a 64 byte salt
	/// </summary>
	public class CreateHMACSHAHash : ICreateHMACHash {
		public const int SHA256SaltSize = 64;
		public const int SHA512SaltSize = 128;

		byte[] GetBytes(string signature) => System.Text.Encoding.UTF8.GetBytes(signature);

		byte[] Create(HMAC sha, string signature, out byte[] salt) {
			if (signature == null) { throw new ArgumentNullException(); }
			var result = sha.ComputeHash(GetBytes(signature));
			salt = sha.Key;
			return result;
		}

		public byte[] Create(string signature, byte[] salt) {
			if (signature == null || salt == null) { throw new ArgumentNullException(); }
			byte[] bytes = GetBytes(signature);
			if (salt.Length == SHA256SaltSize) {
				using (var hmac = new HMACSHA256(salt)) {
					return hmac.ComputeHash(bytes);
				}
			} else if (salt.Length == SHA512SaltSize) {
				using (var hmac = new HMACSHA512(salt)) {
					return hmac.ComputeHash(bytes);
				}
			} else {
				throw new ArgumentNullException($"Invalid byte array length for salt; expecting {SHA256SaltSize} or {SHA512SaltSize} bytes");
			}
		}

		public byte[] Create256(string signature, out byte[] salt) {
			using (var sha = new HMACSHA256()) {
				return Create(sha, signature, out salt);
			}
		}

		public byte[] Create512(string signature, out byte[] salt) {
			using (var sha = new HMACSHA512()) {
				return Create(sha, signature, out salt);
			}
		}
	}
}
