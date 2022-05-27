using System;
using NBitcoin.Crypto;

namespace FioSharp.Secp256k1
{
    public class ShaManager
    {
        public static byte[] GetSha256Hash(byte[] data)
        {
            return Hashes.SHA256(data);
        }
    }
}
