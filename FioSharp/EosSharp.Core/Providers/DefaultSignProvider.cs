using FioSharp.Secp256k1;
using FioSharp.Core.Helpers;
using FioSharp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FioSharp.Secp256k1.Helpers;

namespace FioSharp.Core.Providers
{
    /// <summary>
    /// Signature provider default implementation that stores private keys in memory
    /// </summary>
    public class DefaultSignProvider : ISignProvider
    {
        private readonly byte[] KeyTypeBytes = Encoding.UTF8.GetBytes("K1");
        private readonly Dictionary<string, byte[]> Keys = new Dictionary<string, byte[]>();

        /// <summary>
        /// Create provider with single private key
        /// </summary>
        /// <param name="privateKey"></param>
        public DefaultSignProvider(string privateKey)
        {
            var privKeyBytes = CryptoHelper.GetPrivateKeyBytesWithoutCheckSum(privateKey);
            var pubKey = CryptoHelper.PubKeyBytesToString(Secp256K1Manager.GetPublicKey(privKeyBytes));
            Keys.Add(pubKey, privKeyBytes);
        }

        /// <summary>
        /// Create provider with list of private keys
        /// </summary>
        /// <param name="privateKeys"></param>
        public DefaultSignProvider(List<string> privateKeys)
        {
            if (privateKeys == null || privateKeys.Count == 0)
                throw new ArgumentNullException("privateKeys");

            foreach(var key in privateKeys)
            {
                var privKeyBytes = CryptoHelper.GetPrivateKeyBytesWithoutCheckSum(key);
                var pubKey = CryptoHelper.PubKeyBytesToString(Secp256K1Manager.GetPublicKey(privKeyBytes));
                Keys.Add(pubKey, privKeyBytes);
            }
        }

        /// <summary>
        /// Create provider with dictionary of encoded key pairs
        /// </summary>
        /// <param name="encodedKeys"></param>
        public DefaultSignProvider(Dictionary<string, string> encodedKeys)
        {
            if (encodedKeys == null || encodedKeys.Count == 0)
                throw new ArgumentNullException("encodedKeys");

            foreach (var keyPair in encodedKeys)
            {
                var privKeyBytes = CryptoHelper.GetPrivateKeyBytesWithoutCheckSum(keyPair.Value);
                Keys.Add(keyPair.Key, privKeyBytes);
            }
        }

        /// <summary>
        /// Create provider with dictionary of  key pair with private key as byte array
        /// </summary>
        /// <param name="keys"></param>
        public DefaultSignProvider(Dictionary<string, byte[]> keys)
        {
            if (keys == null || keys.Count == 0)
                throw new ArgumentNullException("encodedKeys");

            Keys = keys;
        }

        /// <summary>
        /// Get available public keys from signature provider
        /// </summary>
        /// <returns>List of public keys</returns>
        public Task<IEnumerable<string>> GetAvailableKeys()
        {
            return Task.FromResult(Keys.Keys.AsEnumerable());
        }

        /// <summary>
        /// Sign bytes using the signature provider
        /// </summary>
        /// <param name="chainId">EOSIO Chain id</param>
        /// <param name="requiredKeys">required public keys for signing this bytes</param>
        /// <param name="signBytes">signature bytes</param>
        /// <param name="abiNames">abi contract names to get abi information from</param>
        /// <returns>List of signatures per required keys</returns>
        public Task<IEnumerable<string>> Sign(string chainId, IEnumerable<string> requiredKeys, byte[] signBytes, IEnumerable<string> abiNames = null)
        {
            //Console.WriteLine("Required Keys");
            //foreach (string key in requiredKeys)
            //{
            //    Console.WriteLine(key);
            //}
            
            if (requiredKeys == null)
                return Task.FromResult(new List<string>().AsEnumerable());

            var availableAndReqKeys = requiredKeys.Intersect(Keys.Keys);
            //Console.WriteLine("Available Keys");
            //foreach (string key in requiredKeys)
            //{
            //    Console.WriteLine(key);
            //}

            var dataList = new List<byte[]>()
            {
                Hex.HexToBytes(chainId),
                signBytes,
                new byte[32]
            };

            //var hash = Sha256Manager.GetHash(SerializationHelper.Combine(data));
            byte[] data = SerializationHelper.Combine(dataList);
            List<string> sigs = new List<string>();

            foreach (string key in requiredKeys)
            {
                var sign = Secp256K1Manager.SignCompressedCompact(data, Keys[key]);
                var check = new List<byte[]>() { sign, KeyTypeBytes };
                var checksum = Ripemd160Manager.GetHash(SerializationHelper.Combine(check)).Take(4).ToArray();
                var signAndChecksum = new List<byte[]>() { sign, checksum };

                sigs.Add("SIG_K1_" + Base58.Encode(SerializationHelper.Combine(signAndChecksum)));
            }

            IEnumerable<string> enumerableSigs = sigs.AsEnumerable();

            //IEnumerable<string> sigs = availableAndReqKeys.Select(key =>
            //{
                
            //});

            return Task.FromResult(enumerableSigs);
        }
    }
}