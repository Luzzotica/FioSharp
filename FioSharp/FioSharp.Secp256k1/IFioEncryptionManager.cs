using System;
namespace FioSharp.Secp256k1
{
    public interface IFioEncryptionManager
    {
        byte[] EncryptBytes(byte[] secret, byte[] data, byte[] IV = null);
        byte[] DecryptBytes(byte[] secret, byte[] data);
    }
}
