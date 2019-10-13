using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Cryptography.Core {
	public interface ICreateHMACHash {
		byte[] Create(string signature, byte[] salt);
		byte[] Create512(string signature, out byte[] salt);
		byte[] Create256(string signature, out byte[] salt);
	}
}
