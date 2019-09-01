using Albatross.Cryptography.Core;
using System;
using System.Security.Cryptography;

namespace Albatross.Cryptography {
    public class CryptoRNG : ICryptoRNG, IDisposable {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

        public void Dispose() {
            provider.Dispose();
        }

        public byte[] GetBytes(int size) {
            byte[] result = new byte[size];
            provider.GetBytes(result);
            return result;
        }
    }
}
