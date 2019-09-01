using System;

namespace Albatross.Cryptography.Core {
    public static class Extension {
        public static string GetBase64(this ICryptoRNG rng, int size) {
            byte[] data = rng.GetBytes(size);
            return Convert.ToBase64String(data);
        }

		public static bool Match(this byte[] arrayA, byte[] arrayB) {
			if(arrayA != null && arrayB!= null && arrayA.Length == arrayB.Length) {
				for(int i=0; i<arrayA.Length; i++) {
					if (arrayA[i] != arrayB[i]) { return false; }
				}
				return true;
			}
			return false;
		}

		public static string SHA256(this string text) {
			var bytes = new CreateSHA256Hash().Create(text);
			return Convert.ToBase64String(bytes);
		}
		public static string SHA512(this string text) {
			var bytes = new CreateSHA512Hash().Create(text);
			return Convert.ToBase64String(bytes);
		}

		public static string HMACSHAHash(this string text, string salt) {
			byte[] saltArray = System.Convert.FromBase64String(salt);
			var bytes = new CreateHMACSHAHash().Create(text, saltArray);
			return Convert.ToBase64String(bytes);
		}
	}
}
