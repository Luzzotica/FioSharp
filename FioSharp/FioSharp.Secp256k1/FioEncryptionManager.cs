using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NBitcoin.Crypto;

namespace FioSharp.Secp256k1
{
    public class FioEncryptionManager
    {
        static byte[] KeBuf = new byte[32];
        static byte[] IVBuf = new byte[16];
        static byte[] hmacBuf = new byte[32];

        public static byte[] EncryptBytes(byte[] secret, byte[] data, byte[] IV = null)
        {
            byte[] K = Hashes.SHA512(secret);
            Array.Copy(K, KeBuf, 32);
            byte[] Km = K.Skip(32).ToArray();

            // Handle the IV, static values can be used for testing purposes.
            // Otherwise, always generate cryptographically strong random values.
            if (IV == null)
            {
                RandomNumberGenerator.Create().GetBytes(IVBuf);
            }
            else if (IV.Length != 16)
            {
                throw new Exception("IV must be 16 bytes");
            }
            else
            {
                Array.Copy(IV, IVBuf, 16);
            }

            byte[] C;

            // Create our aes encrpytion object, set its values, get the crypto transformer
            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Key = KeBuf;
                aes.IV = IVBuf;
                ICryptoTransform aesEncryptor = aes.CreateEncryptor();
                
                // Instantiate a new MemoryStream object to contain the encrypted bytes
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write))
                    {
                        // Encrypt the data and finish the encryption process
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    C = memoryStream.ToArray();
                }
            }
                

            // Encrypt the data, concatenate it and return it
            hmacBuf = Hashes.HMACSHA256(Km, IVBuf.Concat(C).ToArray());
            return IVBuf.Concat(C).Concat(hmacBuf).ToArray();
        }

        public static byte[] DecryptBytes(byte[] secret, byte[] message)
        {
            byte[] K = Hashes.SHA512(secret);
            Array.Copy(K, KeBuf, 32);
            byte[] Km = K.Skip(32).ToArray();
            Array.Copy(message, IVBuf, 16);
            byte[] C = new byte[message.Length - 48];
            Console.WriteLine(message.Length);
            Array.Copy(message, 16, C, 0, message.Length - 48);
            Array.Copy(message, message.Length - 32, hmacBuf, 0, 32);

            // Side-channel attack protection: First verify the HMAC, then and only then proceed to the decryption step
            byte[] hmacVerify = Hashes.HMACSHA256(Km, IVBuf.Concat(C).ToArray());
            if (!Enumerable.SequenceEqual(hmacBuf, hmacVerify))
            {
                throw new Exception("Decryption failed");
            }

            byte[] decrypted;

            //
            // Create our aes encrpytion object, set its values, get the crypto transformer
            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Key = KeBuf;
                aes.IV = IVBuf;
                ICryptoTransform aesDecryptor = aes.CreateDecryptor();

                // Instantiate a new MemoryStream object to contain the encrypted bytes
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write))
                    {
                        // Encrypt the data and finish the encryption process
                        cryptoStream.Write(C, 0, C.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    decrypted = memoryStream.ToArray();
                }
            }

            return decrypted;
        }
    }
}
