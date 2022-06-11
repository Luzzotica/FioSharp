using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using FioSharp.Secp256k1.Models;
//using Cryptography.ECDSA.Internal.Secp256K1;
//using NBitcoin.Secp256k1;
using NBitcoin;
using NBitcoin.Crypto;

namespace FioSharp.Secp256k1
{
    public class Secp256K1Manager
    {
        /// <summary>Signs a data and returns the signature in compact form.  Returns null on failure.</summary>
        /// <param name="data">The data to sign.  This data is not hashed.  For use with bitcoins, you probably want to double-SHA256 hash this before calling this method.</param>
        /// <param name="seckey">The private key to use to sign the data.</param>
        /// <param name="recoveryId">This will contain the recovery ID needed to retrieve the key from the compact signature using the RecoverKeyFromCompact method.</param> 
        public static byte[] SignCompact(byte[] data, byte[] seckey, out int recoveryId)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(data));
            if (seckey == null)
                throw new ArgumentNullException(nameof(seckey));
            if (seckey.Length != 32)
                throw new ArgumentOutOfRangeException(nameof(seckey));

            // Create a new key from the secret one, double sha the data, and sign it compactly using NBitcoin
            Key key = new Key(seckey);
            uint256 hash = Hashes.DoubleSHA256(data);
            CompactSignature compSig = key.SignCompact(hash);
            recoveryId = compSig.RecoveryId;
            return compSig.Signature;
        }

        /// <summary>
        /// Get compressed and compact signature
        /// </summary>
        /// <param name="data">Raw transaction data</param>
        /// <param name="seckey">Private key (32 bytes)</param>
        /// <returns> 65 bytes compressed / compact</returns>
        public static byte[] SignCompressedCompact(byte[] data, byte[] seckey)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(data));
            if (seckey == null)
                throw new ArgumentNullException(nameof(seckey));
            if (seckey.Length != 32)
                throw new ArgumentOutOfRangeException(nameof(seckey));

            // Create a new key from the secret one, double sha the data, and sign it compactly using NBitcoin
            Key key = new Key(seckey);
            uint256 hash = new uint256(Hashes.SHA256(data)); //Hashes.DoubleSHA256(data);
            
            CompactSignature sig = key.SignCompact(hash);
            
            byte[] output65 = new byte[65];
            output65[0] = (byte)(sig.RecoveryId + 4 + 27);
            Array.Copy(sig.Signature, 0, output65, 1, 64);

            return output65;
        }

        public static byte[] GetPublicKey(byte[] privateKey)
        {
            if (privateKey == null)
                throw new ArgumentNullException(nameof(privateKey));
            if (privateKey.Length != 32)
                throw new ArgumentOutOfRangeException(nameof(privateKey));

            Key key = new Key(privateKey, -1, true);
            return key.PubKey.ToBytes();
        }

        public static byte[] GetSharedKey(byte[] privateKey, byte[] otherPubKey)
        {
            Key key = new Key(privateKey);
            PubKey pubKey = new PubKey(otherPubKey);
            PubKey sharedKey = pubKey.GetSharedPubkey(key);

            // Fio Cuts off first byte (Sign) and takes sha512
            return Hashes.SHA512(sharedKey.ToBytes().Skip(1).ToArray());
        }

        public static byte[] GenerateRandomKey()
        {
            var rand = RandomNumberGenerator.Create();
            var key = new byte[32];
            rand.GetBytes(key);
            return key;
        }

        public static string CreateMnemonic(byte[] entropy)
        {
            Mnemonic m = new Mnemonic(Wordlist.English, entropy: entropy);
            return m.ToString();
        }

        public static byte[] CreatePrivateKeyMnemonic(string mnemonic)
        {
            Mnemonic m = new Mnemonic(mnemonic);
            ExtKey key = m.DeriveExtKey().Derive(new KeyPath("m/44\'/235\'/0\'/0/0"));
            return key.PrivateKey.ToBytes();
        }
    }
}