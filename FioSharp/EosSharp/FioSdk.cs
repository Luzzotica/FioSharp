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

        public static bool isChainCodeValid(string chainCode)
        {
            return Validation.Validate(chainCode, RegexExpressions.CHAIN_TOKEN_CODE);
        }

        public static bool isTokenCodeValid(string tokenCode)
        {
            return Validation.Validate(tokenCode, RegexExpressions.CHAIN_TOKEN_CODE);
        }

        public static bool isFioAddressValid(string fioAddress)
        {
            return Validation.Validate(fioAddress, RegexExpressions.FIO_ADDRESS);
        }

        public static bool isFioDomainValid(string fioDomain)
        {
            return Validation.Validate(fioDomain, RegexExpressions.FIO_DOMAIN);
        }

        public static bool isFioPublicKeyValid(string fioPublicKey)
        {
            return Validation.Validate(fioPublicKey, RegexExpressions.FIO_PUBLIC_KEY);
        }

        public static bool isPublicAddressValid(string publicAddress)
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
            Actor = await GetActorAsync(PublicKey);
            SetupComplete();
        }

        public void SetupComplete() { }

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

        public async Task<string> PushTransaction(List<Core.Api.v1.Action> actions,
            List<Core.Api.v1.Action> contextFreeActions = null,
            List<Extension> transactionExtensions = null)
        {
            var getInfoResult = await Fio.GetInfo();
            var getBlockResult = await Fio.GetBlock(getInfoResult.last_irreversible_block_num.ToString());

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

        public List<PermissionLevel> GetPermissionLevels()
        {
            return new List<PermissionLevel>()
            {
                new PermissionLevel() {actor = "bepto5rwh5gy", permission = "active" }
            };
        }

        public async Task<string> GetActorAsync(string fioPubKey)
        {
            return (await Fio.GetActor(fioPubKey)).actor;
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
    }
}
