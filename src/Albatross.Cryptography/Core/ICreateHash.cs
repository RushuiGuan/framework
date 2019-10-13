using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Cryptography.Core {
	public interface ICreateHash {
		byte[] Create(string text);
		int HashSize { get; }
	}
}
