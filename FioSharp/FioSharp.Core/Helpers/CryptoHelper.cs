using FioSharp.Secp256k1;
using FioSharp.Secp256k1.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FioSharp.Core.Helpers
{
    /// <summary>
    /// Helper class with crypto functions
    /// </summary>
    public class CryptoHelper
    {
        public static readonly int PUB_KEY_DATA_SIZE = 33;
        public static readonly int PRIV_KEY_DATA_SIZE = 32;
        public static readonly int SIGN_KEY_DATA_SIZE = 64;

        /// <summary>
        /// KeyPair with a private and public key
        /// </summary>
        public class KeyPair
        {
            public string PrivateKey { get; set; }
            public string PublicKey { get; set; }
        }

        /// <summary>
        /// Generate a new key pair based on a key type
        /// </summary>
        /// <param name="keyType">Optional key type. (sha256x2, R1)</param>
        /// <returns>key pair</returns>
        public static KeyPair GenerateKeyPair(string keyType = "sha256x2")
        {
            var key = Secp256K1Manager.GenerateRandomKey();
            var pubKey = Secp256K1Manager.GetPublicKey(key);

            if (keyType != "sha256x2" && keyType != "R1")
                throw new Exception("invalid key type.");

            return new KeyPair()
            {
                PrivateKey = KeyToString(key, keyType, keyType == "R1" ? "PVT_R1_" : null),
                PublicKey = KeyToString(pubKey, keyType != "sha256x2" ? keyType : null, keyType == "R1" ? "PUB_R1_" : "FIO")
            };
        }

        /// <summary>
        /// Get private key bytes without is checksum
        /// </summary>
        /// <param name="privateKey">private key</param>
        /// <returns>byte array</returns>
        public static byte[] GetPrivateKeyBytesWithoutCheckSum(string privateKey)
        {
            if (privateKey.StartsWith("PVT_R1_"))
                return PrivKeyStringToBytes(privateKey).Take(PRIV_KEY_DATA_SIZE).ToArray();
            else
                return PrivKeyStringToBytes(privateKey).Skip(1).Take(PRIV_KEY_DATA_SIZE).ToArray();
        }

        /// <summary>
        /// Get public key bytes without is checksum
        /// </summary>
        /// <param name="publicKey">public key</param>
        /// <returns>byte array</returns>
        public static byte[] GetPublicKeyBytesWithoutCheckSum(string publicKey)
        {
            return PubKeyStringToBytes(publicKey).Take(PUB_KEY_DATA_SIZE).ToArray();
        }

        /// <summary>
        /// Convert byte array to encoded public key string
        /// </summary>
        /// <param name="keyBytes">public key bytes</param>
        /// <param name="keyType">Optional key type. (sha256x2, R1, K1)</param>
        /// <param name="prefix">Optional prefix to public key</param>
        /// <returns>encoded public key</returns>
        public static string PubKeyBytesToString(byte[] keyBytes, string keyType = null, string prefix = "FIO")
        {
            return KeyToString(keyBytes, keyType, prefix);
        }

        /// <summary>
        /// Convert byte array to encoded private key string
        /// </summary>
        /// <param name="keyBytes">public key bytes</param>
        /// <param name="keyType">Optional key type. (sha256x2, R1, K1)</param>
        /// <param name="prefix">Optional prefix to public key</param>
        /// <returns>encoded private key</returns>
        public static string PrivKeyBytesToString(byte[] keyBytes, string keyType = "R1", string prefix = "PVT_R1_")
        {
            return KeyToString(keyBytes, keyType, prefix);
        }

        /// <summary>
        /// Convert byte array to encoded signature string
        /// </summary>
        /// <param name="signBytes">signature bytes</param>
        /// <param name="keyType">Optional key type. (sha256x2, R1, K1)</param>
        /// <param name="prefix">Optional prefix to public key</param>
        /// <returns>encoded signature</returns>
        public static string SignBytesToString(byte[] signBytes, string keyType = "K1", string prefix = "SIG_K1_")
        {
            return KeyToString(signBytes, keyType, prefix);
        }

        /// <summary>
        /// Convert encoded public key to byte array
        /// </summary>
        /// <param name="key">encoded public key</param>
        /// <param name="prefix">Optional prefix on key</param>
        /// <returns>public key bytes</returns>
        public static byte[] PubKeyStringToBytes(string key, string prefix = "FIO")
        {
            if(key.StartsWith("PUB_R1_"))
            {
                return StringToKey(key.Substring(7), PUB_KEY_DATA_SIZE, "R1");
            }
            else if(key.StartsWith(prefix))
            {
                return StringToKey(key.Substring(prefix.Length), PUB_KEY_DATA_SIZE);
            }
            else
            {
                throw new Exception("unrecognized public key format.");
            }
        }

        /// <summary>
        /// Convert encoded public key to byte array
        /// </summary>
        /// <param name="key">encoded public key</param>
        /// <param name="prefix">Optional prefix on key</param>
        /// <returns>public key bytes</returns>
        public static byte[] PrivKeyStringToBytes(string key)
        {
            if (key.StartsWith("PVT_R1_"))
                return StringToKey(key.Substring(7), PRIV_KEY_DATA_SIZE, "R1");
            else
                return StringToKey(key, PRIV_KEY_DATA_SIZE, "sha256x2");
        }

        /// <summary>
        /// Convert encoded signature to byte array
        /// </summary>
        /// <param name="sign">encoded signature</param>
        /// <returns>signature bytes</returns>
        public static byte[] SignStringToBytes(string sign)
        {
            if (sign.StartsWith("SIG_K1_"))
                return StringToKey(sign.Substring(7), SIGN_KEY_DATA_SIZE, "K1");
            if (sign.StartsWith("SIG_R1_"))
                return StringToKey(sign.Substring(7), SIGN_KEY_DATA_SIZE, "R1");
            else
                throw new Exception("unrecognized signature format.");
        }

        /// <summary>
        /// Convert Pub/Priv key or signature to byte array
        /// </summary>
        /// <param name="key">generic key</param>
        /// <param name="size">Key size</param>
        /// <param name="keyType">Optional key type. (sha256x2, R1, K1)</param>
        /// <returns>key bytes</returns>
        public static byte[] StringToKey(string key, int size, string keyType = null) 
        {
            var keyBytes = Base58.Decode(key);
            byte[] digest = null;
            int skipSize = 0;

            if(keyType == "sha256x2")
            {
                // skip version
                skipSize = 1;
                digest = ShaManager.GetSha256Hash(ShaManager.GetSha256Hash(keyBytes.Take(size + skipSize).ToArray()));
            }
            else if(!string.IsNullOrWhiteSpace(keyType))
            {
                // skip K1 recovery param
                if(keyType == "K1")
                    skipSize = 1;

                digest = Ripemd160Manager.GetHash(SerializationHelper.Combine(new List<byte[]>() {
                    keyBytes.Take(size + skipSize).ToArray(),
                    Encoding.UTF8.GetBytes(keyType)
                }));
            }
            else
            {
                digest = Ripemd160Manager.GetHash(keyBytes.Take(size).ToArray());
            }

            if (!keyBytes.Skip(size + skipSize).SequenceEqual(digest.Take(4)))
            {
                throw new Exception("checksum doesn't match.");
            }
            return keyBytes;
        }

        /// <summary>
        /// Convert key byte array to encoded generic key
        /// </summary>
        /// <param name="key">key byte array</param>
        /// <param name="keyType">Key type. (sha256x2, R1, K1)</param>
        /// <param name="prefix">Optional prefix</param>
        /// <returns></returns>
        public static string KeyToString(byte[] key, string keyType, string prefix = null)
        {
            byte[] digest = null;

            if (keyType == "sha256x2")
            {
                digest = ShaManager.GetSha256Hash(ShaManager.GetSha256Hash(SerializationHelper.Combine(new List<byte[]>() {
                    new byte[] { 128 },
                    key
                })));
            }
            else if (!string.IsNullOrWhiteSpace(keyType))
            {
                digest = Ripemd160Manager.GetHash(SerializationHelper.Combine(new List<byte[]>() {
                    key,
                    Encoding.UTF8.GetBytes(keyType)
                }));
            }
            else
            {
                digest = Ripemd160Manager.GetHash(key);
            }

            if(keyType == "sha256x2")
            {
                return (prefix ?? "") + Base58.Encode(SerializationHelper.Combine(new List<byte[]>() {
                    new byte[] { 128 },
                    key,
                    digest.Take(4).ToArray()
                }));
            }
            else
            {
                return (prefix ?? "") + Base58.Encode(SerializationHelper.Combine(new List<byte[]>() {
                    key,
                    digest.Take(4).ToArray()
                }));
            }
        }
    }
}
