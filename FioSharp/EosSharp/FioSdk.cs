using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FioSharp.Core;
using FioSharp.Core.Api.v1;
using FioSharp.Core.Helpers;
using FioSharp.Core.Interfaces;
using FioSharp.Core.Providers;
using FioSharp.Models;
using FioSharp.Secp256k1;
using FioSharp.Util;

namespace FioSharp
{
    public class FioSdk
    {
        public static int SUFUnit = 1000000000;

        public static MnemonicKey CreatePrivateKey(byte[] entropy = null)
        {
            string mnemonic = Secp256K1Manager.CreateMnemonic(entropy);
            return CreatePrivateKeyMnemonic(mnemonic);
        }

        public static MnemonicKey CreatePrivateKeyMnemonic(string mnemonic)
        {
            byte[] privateKey = Secp256K1Manager.CreatePrivateKeyMnemonic(mnemonic);
            return new MnemonicKey()
            {
                fioPrivateKey = CryptoHelper.PrivKeyBytesToString(privateKey),
                mnemonic = mnemonic
            };
        }

        public static string DerivePublicKey(string fioPrivateKey)
        {
            byte[] privateKeyBytes = CryptoHelper.GetPrivateKeyBytesWithoutCheckSum(fioPrivateKey);
            return CryptoHelper.PubKeyBytesToString(Secp256K1Manager.GetPublicKey(privateKeyBytes));
        }

        public static bool IsChainCodeValid(string chainCode)
        {
            return Validation.Validate(chainCode, RegexExpressions.CHAIN_TOKEN_CODE);
        }

        public static bool IsTokenCodeValid(string tokenCode)
        {
            return Validation.Validate(tokenCode, RegexExpressions.CHAIN_TOKEN_CODE);
        }

        public static bool IsFioAddressValid(string fioAddress)
        {
            return Validation.Validate(fioAddress, RegexExpressions.FIO_ADDRESS);
        }

        public static bool IsFioDomainValid(string fioDomain)
        {
            return Validation.Validate(fioDomain, RegexExpressions.FIO_DOMAIN);
        }

        public static bool IsFioPublicKeyValid(string fioPublicKey)
        {
            return Validation.Validate(fioPublicKey, RegexExpressions.FIO_PUBLIC_KEY);
        }

        public static bool IsPublicAddressValid(string publicAddress)
        {
            return Validation.Validate(publicAddress, RegexExpressions.NATIVE_BLOCKCHAIN_ADDRESS);
        }

        public static double amountToSUF(double amount)
        {
            double floor = Math.Floor(amount);
            double tempResult = floor * SUFUnit;

            double remainder = amount % 1;
            double remainderResult = remainder * SUFUnit;
            double floorRemainder = Math.Floor(remainderResult);

            return tempResult + floorRemainder;
        }

        public static double SUFToAmount(double suf)
        {
            return suf / SUFUnit;
        }

        public string PrivateKey { get; private set; }
        public string PublicKey { get; private set; }
        public string TechnologyProviderId { get; private set; }
        public string Actor { get; private set; }
        byte[] privateKeyBytes;
        byte[] publicKeyBytes;

        public Fio Fio { get; private set; }

        public Action<bool> setupComplete;

        public FioSdk(string privateKey,
            string publicKey,
            string baseUrl="",
            IHttpHandler httpHandler=null,
            string technologyProviderId="",
            bool test=false)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
            TechnologyProviderId = technologyProviderId;

            string url = baseUrl;
            if (url.Equals(""))
            {
                url = test ? "https://testnet.fioprotocol.io:443" : "peer.fio.alohaeos.com:9876";
            }
            string chainId = test ? "b20901380af44ef59c5918439a1f9a41d83669020319a80574b804a5f95cbd7e" :
                "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906";

            var fioConfig = new FioConfigurator()
            {
                SignProvider = new DefaultSignProvider(new List<string>() {
                    privateKey,
                }),

                HttpEndpoint = baseUrl,
                ChainId = chainId
            };

            // Assign our fio config
            Fio = httpHandler == null ? new Fio(fioConfig) : new Fio(fioConfig, httpHandler);
            _ = Setup();
        }

        private async Task Setup()
        {
            Actor = await GetActor(PublicKey);
            setupComplete?.Invoke(true);
        }

        #region DH Encryption and Decryption

        public async Task<string> DHEncrypt(string publicKey, string fioContentType, Dictionary<string, dynamic> content)
        {
            byte[] publicKeyBytes = CryptoHelper.GetPublicKeyBytesWithoutCheckSum(publicKey);
            return await Fio.DHEncrypt(GetPrivateKeyBytes(), publicKeyBytes, fioContentType, content);
        }

        public async Task<Dictionary<string, dynamic>> DHDecrypt(string publicKey, string fioContentType, string content)
        {
            byte[] publicKeyBytes = CryptoHelper.GetPublicKeyBytesWithoutCheckSum(publicKey);
            return await Fio.DHDecrypt(GetPrivateKeyBytes(), publicKeyBytes, fioContentType, content);
        }

        #endregion

        #region Transaction Handling

        public async Task<string> PushTransaction(List<Core.Api.v1.Action> actions,
            List<Core.Api.v1.Action> contextFreeActions = null,
            List<Extension> transactionExtensions = null)
        {
            var getInfoResult = await Fio.Api.GetInfo();
            var getBlockResult = await Fio.Api.GetBlock(getInfoResult.last_irreversible_block_num.ToString());

            contextFreeActions = contextFreeActions == null ? new List<Core.Api.v1.Action>() : contextFreeActions;
            transactionExtensions = transactionExtensions == null ? new List<Extension>() : transactionExtensions;

            var trx = new Transaction()
            {
                //trx headers
                expiration = getInfoResult.head_block_time.AddSeconds(60), //expire Seconds
                ref_block_num = (UInt16)(getInfoResult.last_irreversible_block_num & 0xFFFF),
                ref_block_prefix = getBlockResult.ref_block_prefix,
                // trx info
                max_net_usage_words = 0,
                max_cpu_usage_ms = 0,
                delay_sec = 0,
                context_free_actions = contextFreeActions,
                transaction_extensions = transactionExtensions,
                actions = actions
            };

            return await Fio.CreateTransaction(trx);
        }

        #endregion

        #region Api Endpoints

        public async Task<GetFioBalanceResponse> GetFioBalance(string fioPublicKey)
        {
            return await Fio.Api.GetFioBalance(fioPublicKey);
        }

        public async Task<GetFioNamesResponse> GetFioNames(string fioPublicKey)
        {
            return await Fio.Api.GetFioNames(fioPublicKey);
        }

        public async Task<GetFioAddressesResponse> GetFioAddresses(string fioPublicKey, int limit, int offset)
        {
            return await Fio.Api.GetFioAddresses(fioPublicKey, limit, offset);
        }

        public async Task<GetFioDomainsResponse> GetFioDomains(string fioPublicKey, int limit, int offset)
        {
            return await Fio.Api.GetFioDomains(fioPublicKey, limit, offset);
        }

        public async Task<AvailCheckResponse> AvailCheck(string fioName)
        {
            return await Fio.Api.AvailCheck(fioName);
        }

        public async Task<GetPubAddressRequest> GetPublicAddress(string fioAddress, string chainCode, string tokenCode)
        {
            return await Fio.Api.GetPublicAddress(fioAddress, chainCode, tokenCode);
        }

        public async Task<GetPubAddressesResponse> GetPublicAddresses(string fioAddress, int limit, int offset)
        {
            return await Fio.Api.GetPublicAddresses(fioAddress, limit, offset);
        }

        public async Task<GetFioRequestsResponse> GetSentFioRequests(string fioPublicKey, int limit, int offset)
        {
            return await Fio.Api.GetSentFioRequests(fioPublicKey, limit, offset);
        }

        public async Task<GetFioRequestsResponse> GetReceivedFioRequests(string fioPublicKey, int limit, int offset)
        {
            return await Fio.Api.GetReceivedFioRequests(fioPublicKey, limit, offset);
        }

        public async Task<GetFioRequestsResponse> GetPendingFioRequests(string fioPublicKey, int limit, int offset)
        {
            return await Fio.Api.GetPendingFioRequests(fioPublicKey, limit, offset);
        }

        public async Task<GetFioRequestsResponse> GetCancelledFioRequests(string fioPublicKey, int limit, int offset)
        {
            return await Fio.Api.GetCancelledFioRequests(fioPublicKey, limit, offset);
        }

        public async Task<GetObtDataResponse> GetObtData(string fioPublicKey, int limit, int offset)
        {
            return await Fio.Api.GetObtData(fioPublicKey, limit, offset);
        }

        public async Task<GetLocksResponse> GetLocks(string fioPublicKey)
        {
            return await Fio.Api.GetLocks(fioPublicKey);
        }

        public async Task<GetFeeResponse> GetFee(string endpoint, string fioAddress)
        {
            return await Fio.Api.GetFee(endpoint, fioAddress);
        }

        public async Task<GetNftsResponse> GetNftsFioAddress(string fioAddress, int limit, int offset)
        {
            return await Fio.Api.GetNftsFioAddress(fioAddress, limit, offset);
        }

        public async Task<GetNftsResponse> GetNftsContract(string contractAddress, string chainCode, string tokenId, int limit, int offset)
        {
            return await Fio.Api.GetNftsContract(contractAddress, chainCode, tokenId, limit, offset);
        }

        public async Task<GetNftsResponse> GetNftsHash(string hash, int limit, int offset)
        {
            return await Fio.Api.GetNftsHash(hash, limit, offset);
        }

        public async Task<string> GetActor(string fioPubKey)
        {
            return (await Fio.Api.GetActor(fioPubKey)).actor;
        }

        public async Task<GetAccountResponse> GetAccount(string accountName)
        {
            return await Fio.Api.GetAccount(accountName);
        }

        /// <summary>
        /// Query for blockchain information
        /// </summary>
        /// <returns>Blockchain information</returns>
        public async Task<GetInfoResponse> GetInfo()
        {
            return await Fio.Api.GetInfo();
        }

        /// <summary>
        /// Query for blockchain block information
        /// </summary>
        /// <param name="blockNumOrId">block number or id to query information</param>
        /// <returns>block information</returns>
        public async Task<GetBlockResponse> GetBlock(string blockNumOrId)
        {
            return await Fio.Api.GetBlock(blockNumOrId);
        }

        /// <summary>
        /// Query for smart contract abi detailed information
        /// </summary>
        /// <param name="accountName">smart contract account name</param>
        /// <returns></returns>
        public async Task<Abi> GetAbi(string accountName, bool reload = false)
        {
            return await Fio.Api.GetAbi(accountName, reload);
        }

        /// <summary>
        /// Query for smart contract abi detailed information
        /// </summary>
        /// <param name="accountName">smart contract account name</param>
        /// <returns>smart contract abi information as Base64FcString</returns>
        public async Task<GetRawAbiResponse> GetRawAbi(string accountName, bool reload = false)
        {
            return await Fio.Api.GetRawAbi(accountName, reload);
        }

        public async Task<GetProducersResponse> GetProducers(string limit, string lower_bound)
        {
            return await Fio.Api.GetProducers(limit, lower_bound);
        }

        #endregion

        #region Getters and Setters

        public List<PermissionLevel> GetPermissionLevels()
        {
            return new List<PermissionLevel>()
            {
                new PermissionLevel() {actor = "bepto5rwh5gy", permission = "active" }
            };
        }

        public byte[] GetPrivateKeyBytes()
        {
            if (privateKeyBytes != null)
            {
                return privateKeyBytes;
            }

            // Cache the result
            privateKeyBytes = CryptoHelper.GetPrivateKeyBytesWithoutCheckSum(PrivateKey);
            return privateKeyBytes;
        }

        #endregion
    }
}
