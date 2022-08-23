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
    public class FioSdk : IFioSdk
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
                fioPrivateKey = CryptoHelper.KeyToString(privateKey, "sha256x2"),
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

        public static long AmountToSUF(double amount)
        {
            double floor = Math.Floor(amount);
            double tempResult = floor * SUFUnit;

            double remainder = amount % 1;
            double remainderResult = remainder * SUFUnit;
            double floorRemainder = Math.Floor(remainderResult);

            return (long)(tempResult + floorRemainder);
        }

        public static double SUFToAmount(double suf)
        {
            return suf / SUFUnit;
        }

        string privateKey;
        string publicKey;
        string technologyProviderId;
        string actor;
        byte[] privateKeyBytes;

        public Fio Fio { get; private set; }
        public FioApi Api { get
            {
                return Fio.Api;
            }
        }

        public Action<bool> setupComplete;

        public FioSdk(string privateKey,
            string baseUrl,
            string publicKey="",
            IHttpHandler httpHandler=null,
            string technologyProviderId="")
        {
            this.privateKey = privateKey;
            this.publicKey = publicKey;
            if (this.publicKey.Equals(""))
            {
                this.publicKey = FioSdk.DerivePublicKey(privateKey);
            }
            this.technologyProviderId = technologyProviderId;

            var fioConfig = new FioConfigurator()
            {
                SignProvider = new DefaultSignProvider(new List<string>() {
                    privateKey,
                }),

                HttpEndpoint = baseUrl,
            };

            // Assign our fio config
            Fio = httpHandler == null ? new Fio(fioConfig) : new Fio(fioConfig, httpHandler);
        }

        public async Task Init()
        {
            Fio.FioConfig.ChainId = (await Api.GetInfo()).chain_id;
            actor = (await Api.GetActor(publicKey)).actor;
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

        public async Task<string> PushTransaction(ITransaction data)
        {
            Core.Api.v1.Action action = new Core.Api.v1.Action
            {
                account = data.GetContract(),
                name = data.GetName(),
                authorization = GetAuthorization(),
                data = data.ToJsonObject()
            };
            return await PushTransaction(action);
        }

        public async Task<string> PushTransaction(Core.Api.v1.Action action,
            List<Core.Api.v1.Action> contextFreeActions = null,
            List<Extension> transactionExtensions = null)
        {
            return await PushTransaction(new List<Core.Api.v1.Action> { action },
                contextFreeActions: contextFreeActions,
                transactionExtensions: transactionExtensions);
        }

        public async Task<string> PushTransaction(List<Core.Api.v1.Action> actions,
            List<Core.Api.v1.Action> contextFreeActions = null,
            List<Extension> transactionExtensions = null)
        {
            var getInfoResult = await Api.GetInfo();
            var getBlockResult = await Api.GetBlock(getInfoResult.last_irreversible_block_num.ToString());

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

        public List<PermissionLevel> GetAuthorization(string actor = "")
        {
            string a = actor.Equals("") ? GetActor() : actor;
            return new List<PermissionLevel>()
                {
                    new PermissionLevel() {actor = a, permission = "active"}
                };
        }

        #endregion

        #region API

        public async Task<GetFioBalanceResponse> GetFioBalance(string fioPublicKey=null)
        {
            string key = fioPublicKey;
            if (key == null)
            {
                key = GetPublicKey();
            }
            return await GetFioApi().GetFioBalance(key);
        }
        public async Task<GetFioNamesResponse> GetFioNames(string fioPublicKey=null)
        {
            string key = fioPublicKey;
            if (key == null)
            {
                key = GetPublicKey();
            }
            return await GetFioApi().GetFioNames(key);
        }
        public async Task<GetFioAddressesResponse> GetFioAddresses(int limit, int offset, string fioPublicKey = null)
        {
            string key = fioPublicKey;
            if (key == null)
            {
                key = GetPublicKey();
            }
            return await GetFioApi().GetFioAddresses(key, limit, offset);
        }
        public async Task<GetFioDomainsResponse> GetFioDomains(int limit, int offset, string fioPublicKey = null)
        {
            string key = fioPublicKey;
            if (key == null)
            {
                key = GetPublicKey();
            }
            return await GetFioApi().GetFioDomains(key, limit, offset);
        }
        public async Task<GetSentFioRequestsResponse> GetSentFioRequests(int limit, int offset, string fioPublicKey = null)
        {
            string key = fioPublicKey;
            if (key == null)
            {
                key = GetPublicKey();
            }
            return await GetFioApi().GetSentFioRequests(key, limit, offset);
        }
        public async Task<GetReceivedFioRequestsResponse> GetReceivedFioRequests(int limit, int offset, string fioPublicKey = null)
        {
            string key = fioPublicKey;
            if (key == null)
            {
                key = GetPublicKey();
            }
            return await GetFioApi().GetReceivedFioRequests(key, limit, offset);
        }
        public async Task<GetPendingFioRequestsResponse> GetPendingFioRequests(int limit, int offset, string fioPublicKey = null)
        {
            string key = fioPublicKey;
            if (key == null)
            {
                key = GetPublicKey();
            }
            return await GetFioApi().GetPendingFioRequests(key, limit, offset);
        }
        public async Task<GetCancelledFioRequestsResponse> GetCancelledFioRequests(int limit, int offset, string fioPublicKey = null)
        {
            string key = fioPublicKey;
            if (key == null)
            {
                key = GetPublicKey();
            }
            return await GetFioApi().GetCancelledFioRequests(key, limit, offset);
        }
        public async Task<GetObtDataResponse> GetObtData(int limit, int offset, string fioPublicKey = null)
        {
            string key = fioPublicKey;
            if (key == null)
            {
                key = GetPublicKey();
            }
            return await GetFioApi().GetObtData(key, limit, offset);
        }
        public async Task<GetLocksResponse> GetLocks(string fioPublicKey=null)
        {
            string key = fioPublicKey;
            if (key == null)
            {
                key = GetPublicKey();
            }
            return await GetFioApi().GetLocks(key);
        }
        public async Task<GetActorResponse> GetActor(string fioPublicKey=null)
        {
            string key = fioPublicKey;
            if (key == null)
            {
                key = GetPublicKey();
            }
            return await GetFioApi().GetActor(key);
        }

        #endregion

        #region Getters and Setters

        public Fio GetFio()
        {
            return Fio;
        }

        public FioApi GetFioApi()
        {
            return Fio.Api;
        }

        public string GetPublicKey()
        {
            return publicKey;
        }

        public string GetTehcnologyProviderId()
        {
            return technologyProviderId;
        }

        public string GetActor()
        {
            return actor;
        }

        private byte[] GetPrivateKeyBytes()
        {
            if (privateKeyBytes != null)
            {
                return privateKeyBytes;
            }

            // Cache the result
            privateKeyBytes = CryptoHelper.GetPrivateKeyBytesWithoutCheckSum(privateKey);
            return privateKeyBytes;
        }

        #endregion
    }
}
