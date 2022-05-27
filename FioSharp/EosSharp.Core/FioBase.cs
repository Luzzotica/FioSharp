using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Helpers;
using EosSharp.Core.Interfaces;
using FioSharp.Secp256k1;
using NBitcoin.Crypto;
using static EosSharp.Core.Helpers.FioHelper;

namespace EosSharp.Core
{
    public class FioBase : EosBase
    {
        private const string FIO_REQ_OBT_CONTRACT = "fio.reqobt";

        public FioBase(EosConfigurator config, IHttpHandler httpHandler) :
            base(config, httpHandler)
        {
        }

        /// <summary>
        /// Takes the private and public keys provided and uses diffie hellman to encrypt
        /// the content provided after serializing it.
        /// Returns the encrypted bytes as a base64 string.
        /// </summary>
        /// <param name="privateKey">The private key</param>
        /// <param name="otherPubKey">The public key of the other individual</param>
        /// <param name="fioContentType">The type of content that is being encrypted</param>
        /// <param name="content">The content, structured properly.</param>
        /// <param name="IV">A strongly random 16 byte array. Leave null unless testing.</param>
        /// <returns></returns>
        public async Task<string> DHEncrypt(byte[] privateKey, byte[] otherPubKey, FioContentType fioContentType, Dictionary<string, object> content, byte[] IV = null)
        {
            Abi abi = await AbiSerializer.GetAbi(FIO_REQ_OBT_CONTRACT);
            byte[] message = AbiSerializer.SerializeStructData(
                GetFioAbiStruct(fioContentType),
                content,
                abi
            );
            byte[] secret = Secp256K1Manager.GetSharedKey(privateKey, otherPubKey);
            byte[] encrypted = FioEncryptionManager.GetInstance().EncryptBytes(secret, message, IV);
            return Convert.ToBase64String(encrypted);
        }

        public async Task<Dictionary<string, object>> DHDecrypt(byte[] privateKey, byte[] otherPubKey, FioContentType fioContentType, string content)
        {
            byte[] secret = Secp256K1Manager.GetSharedKey(privateKey, otherPubKey);
            byte[] contentBytes = Convert.FromBase64String(content);
            byte[] decrypted = FioEncryptionManager.GetInstance().DecryptBytes(secret, contentBytes);
            Console.WriteLine("Swag");
            Abi abi = await AbiSerializer.GetAbi(FIO_REQ_OBT_CONTRACT);
            return AbiSerializer.DeserializeStructData(GetFioAbiStruct(fioContentType), decrypted, abi);
        }
    }
}
