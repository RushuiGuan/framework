using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Cryptography.Core {
    public interface ICryptoRNG {
        byte[] GetBytes(int size);
    }
}
