using FioSharp.Core;
using FioSharp.Core.Api.v1;
using FioSharp.Core.Helpers;
using FioSharp.Core.Interfaces;
using FioSharp.Core.Providers;
using FioSharp.Secp256k1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FioSharp.Core
{
    /// <summary>
    /// Client wrapper to interact with the fio blockchains.
    /// </summary>
    public class FioBase
    {
        public FioConfigurator FioConfig { get; private set; }
        public FioApi Api { get; private set; }
        public AbiSerializationProvider AbiSerializer { get; private set; }

        /// <summary>
        /// Client wrapper constructor.
        /// </summary>
        /// <param name="config">Configures client parameters</param>
        /// <param name="httpHandler">Http handler implementation</param>
        public FioBase(FioConfigurator config, IHttpHandler httpHandler)
        {
            FioConfig = config;
            if (FioConfig == null)
            {
                throw new ArgumentNullException("config");
            }
            Api = new FioApi(FioConfig, httpHandler);
            AbiSerializer = new AbiSerializationProvider(Api);
        }

        #region Transaction Methods

        /// <summary>
        /// Creates a signed transaction using the signature provider and broadcasts it to the network
        /// </summary>
        /// <param name="trx">Transaction to send</param>
        /// <param name="requiredKeys">Override required keys to sign transaction</param>
        /// <returns>transaction id</returns>
        public async Task<string> CreateTransaction(Transaction trx)
        {
            var signedTrx = await SignTransaction(trx);
            return await BroadcastTransaction(signedTrx);
        }

        /// <summary>
        /// Creates a signed transaction using the signature provider
        /// </summary>
        /// <param name="trx">Transaction to sign</param>
        /// <returns>transaction id</returns>
        public async Task<SignedTransaction> SignTransaction(Transaction trx)
        {
            if (trx == null)
                throw new ArgumentNullException("Transaction");

            if (FioConfig.SignProvider == null)
                throw new ArgumentNullException("SignProvider");

            GetInfoResponse getInfoResult = null;
            string chainId = FioConfig.ChainId;

            if (string.IsNullOrWhiteSpace(chainId))
            {
                getInfoResult = await Api.GetInfo();
                chainId = getInfoResult.chain_id;
            }

            if (trx.expiration == DateTime.MinValue ||
               trx.ref_block_num == 0 ||
               trx.ref_block_prefix == 0)
            {
                if (getInfoResult == null)
                    getInfoResult = await Api.GetInfo();

                var taposBlockNum = getInfoResult.head_block_num - (int)FioConfig.blocksBehind;
                var getBlockResult = await Api.GetBlock(taposBlockNum.ToString());
                trx.expiration = getBlockResult.timestamp.AddSeconds(FioConfig.ExpireSeconds);
                trx.ref_block_num = (UInt16)(getBlockResult.block_num & 0xFFFF);
                trx.ref_block_prefix = getBlockResult.ref_block_prefix;
                //if ((taposBlockNum - getInfoResult.last_irreversible_block_num) < 2)
                //{

                //}
                //else
                //{
                //    var getBlockHeaderState = await Api.GetBlockHeaderState(new GetBlockHeaderStateRequest()
                //    {
                //        block_num_or_id = taposBlockNum.ToString()
                //    });
                //    trx.expiration = getBlockHeaderState.header.timestamp.AddSeconds(FioConfig.ExpireSeconds);
                //    trx.ref_block_num = (UInt16)(getBlockHeaderState.block_num & 0xFFFF);
                //    trx.ref_block_prefix = Convert.ToUInt32(SerializationHelper.ReverseHex(getBlockHeaderState.id.Substring(16, 8)), 16);
                //}
            }

            // Serialize the transaction, get our required keys, and get the abi for the account
            var packedTrx = await AbiSerializer.SerializePackedTransaction(trx);
            List<string> requiredKeys = (await FioConfig.SignProvider.GetAvailableKeys()).ToList();
            IEnumerable<string> abis = null;
            if (trx.actions != null)
                abis = trx.actions.Select(a => a.account);

            // Sign the transaction and return it
            var signatures = await FioConfig.SignProvider.Sign(chainId, requiredKeys, packedTrx, abis);
            return new SignedTransaction()
            {
                Signatures = signatures,
                PackedTransaction = packedTrx
            };
        }

        /// <summary>
        /// Broadcast signed transaction to the network
        /// </summary>
        /// <param name="strx">Signed transaction to send</param>
        /// <returns></returns>
        public async Task<string> BroadcastTransaction(SignedTransaction strx)
        {
            if (strx == null)
                throw new ArgumentNullException("SignedTransaction");

            PushTransactionResponse result = await Api.PushTransaction(strx.Signatures.ToArray(),
                SerializationHelper.ByteArrayToHexString(strx.PackedTransaction));
            return result.transaction_id;
        }

        #endregion

        #region DH Encryption and Decryption

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
        public async Task<string> DHEncrypt(byte[] privateKey, byte[] otherPubKey, string fioContentType, Dictionary<string, object> content, byte[] IV = null)
        {
            Abi abi = await AbiSerializer.GetAbi(FioHelper.FIO_REQ_OBT_CONTRACT);
            byte[] message = AbiSerializer.SerializeStructData(
                FioHelper.GetFioAbiStruct(fioContentType),
                content,
                abi
            );
            byte[] secret = Secp256K1Manager.GetSharedKey(privateKey, otherPubKey);
            byte[] encrypted = FioEncryptionManager.EncryptBytes(secret, message, IV);
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Takes the private and public keys provided and uses diffie hellman to decrypt
        /// the content provided and deserialize it into a dictionary and return it.
        /// </summary>
        /// <param name="privateKey">The private key</param>
        /// <param name="otherPubKey">The public key of the other individual</param>
        /// <param name="fioContentType">The type of content that is being decrypted</param>
        /// <param name="content">Base64 string of content to be decrypted and deserialized</param>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> DHDecrypt(byte[] privateKey, byte[] otherPubKey, string fioContentType, string content)
        {
            byte[] secret = Secp256K1Manager.GetSharedKey(privateKey, otherPubKey);
            byte[] contentBytes = Convert.FromBase64String(content);
            byte[] decrypted = FioEncryptionManager.DecryptBytes(secret, contentBytes);
            Abi abi = await AbiSerializer.GetAbi(FioHelper.FIO_REQ_OBT_CONTRACT);
            return AbiSerializer.DeserializeStructData(FioHelper.GetFioAbiStruct(fioContentType), decrypted, abi);
        }

        #endregion
    }
}
