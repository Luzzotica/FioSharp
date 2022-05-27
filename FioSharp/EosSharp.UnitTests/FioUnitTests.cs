using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EosSharp.Core;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Helpers;
using EosSharp.Core.Providers;
using FioSharp.Secp256k1;
using NBitcoin;
using NBitcoin.Crypto;
using Newtonsoft.Json;
using NUnit.Framework;

namespace EosSharp.UnitTests
{
    public class FioUnitTests
    {
        //EosUnitTestCases EosUnitTestCases;
        FioBase Fio { get; set; }

        [SetUp]
        public void Setup()
        {
            var eosConfig = new EosConfigurator()
            {
                SignProvider = new DefaultSignProvider(new List<string>() {
                    "5JtUScZK2XEp3g9gh7F8bwtPTRAkASmNrrftmx4AxDKD5K4zDnr",
                    "5JRC9Kwmeo6CLPKx7TsJ4UybApmLcfxSJKgSMQi3QNtAh88r4Zu"
                }),

                //HttpEndpoint = "https://nodes.eos42.io", //Mainnet
                //ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906"

                HttpEndpoint = "https://testnet.fioprotocol.io:443",
                ChainId = "b20901380af44ef59c5918439a1f9a41d83669020319a80574b804a5f95cbd7e" // Testnet
            };
            //var eosApi = new EosApi(eosConfig, new HttpHandler());

            Fio = new Eos(eosConfig);
            
            privateKeyAlice = Hashes.SHA256(Encoding.ASCII.GetBytes("alice"));
            publicKeyAlice = new Key(privateKeyAlice).PubKey.ToBytes();
            privateKeyBob = Hashes.SHA256(Encoding.ASCII.GetBytes("bob"));
            publicKeyBob = new Key(privateKeyBob).PubKey.ToBytes();

            //Console.WriteLine("--- KEYS ---");
            //Console.WriteLine(SerializationHelper.ByteArrayToHexString(privateKeyAlice));
            //Console.WriteLine(SerializationHelper.ByteArrayToHexString(privateKeyBob));
            //Console.WriteLine(SerializationHelper.ByteArrayToHexString(publicKeyAlice));
            //Console.WriteLine(SerializationHelper.ByteArrayToHexString(publicKeyBob));
        }

        Dictionary<string, object> newFundsContent = new Dictionary<string, object>() {
            { "payee_public_address", "purse.alice" },
            { "amount", "1" },
            { "chain_code", "FIO" },
            { "token_code", "FIO" },
            { "memo", null },
            { "hash", null },
            { "offline_url", null }
        };

        byte[] privateKeyAlice;
        byte[] publicKeyAlice;
        byte[] privateKeyBob;
        byte[] publicKeyBob;

        //f300888ca4f512cebdc0020ff0f7224c
        byte[] IV = new byte[] { 0xf3, 0x00, 0x88, 0x8c, 0xa4, 0xf5, 0x12, 0xce, 0xbd, 0xc0, 0x02, 0x0f, 0xf0, 0xf7, 0x22, 0x4c };
        string newFundsContentCipherBase64 = "8wCIjKT1Es69wAIP8PciTOB8F09qqDGdsq0XriIWcOkqpZe9q4FwKu3SGILtnAWtJGETbcAqd3zX7NDptPUQsS1ZfEPiK6Hv0nJyNbxwiQc=";

    //    SHARED SECRET
    //console.log src/encryption-check.ts:27
    //  <Buffer a7 1b 4e c5 a9 57 79 26 a1 d2 aa 1d 9d 99 32 7f d3 b6 8f 6a 1e a5 97 20 0a 0d 89 0b d3 33 1d f3 00 a2 d4 9f ec 0b 2b 3e 69 69 ce 92 63 c5 d6 cf 47 c1... 14 more bytes>
    //console.log src/encryption-check.ts:29
    //  <Buffer 5c 38 12 dc 9e cd 2e 29 dc 9a 20 aa b9 06 9d f9 40 ab b0 5f cd 10 88 dc ae b6 05 90 e2 c7 9e 41 41 b7 48 43 04 f8 93 e9 d9 93 8e 12 52 47 de 6d 61 8e ... 14 more bytes>
    //console.log src/encryption-check.ts:31
    //  <Buffer 5c 38 12 dc 9e cd 2e 29 dc 9a 20 aa b9 06 9d f9 40 ab b0 5f cd 10 88 dc ae b6 05 90 e2 c7 9e 41>
    //console.log src/encryption-check.ts:33
    //  <Buffer 41 b7 48 43 04 f8 93 e9 d9 93 8e 12 52 47 de 6d 61 8e 5f 50 ba 1a 13 4b 67 80 61 d1 0a b5 26 8b>

        [Test]
        public async Task TestDHEncrypt()
        {
            try { 
                string cipherAliceBase64 = await Fio.DHEncrypt(privateKeyAlice,
                    publicKeyBob,
                    Core.Helpers.FioHelper.FioContentType.newFundsContent,
                    newFundsContent);
                Assert.AreEqual(newFundsContentCipherBase64, cipherAliceBase64);

                string cipherBobBase64 = await Fio.DHEncrypt(privateKeyBob,
                    publicKeyAlice,
                    Core.Helpers.FioHelper.FioContentType.newFundsContent,
                    newFundsContent,
                    IV);
                Assert.AreEqual(newFundsContentCipherBase64, cipherBobBase64);
            }
            catch (Exception e)
            {
                Assert.Fail("Expected no exception, but got: " + e);
            }
        }

        [Test]
        public async Task TestDHDecrypt()
        {
            try
            {
                Dictionary<string, object> newFundsContentAlice = await Fio.DHDecrypt(privateKeyAlice,
                    publicKeyBob,
                    Core.Helpers.FioHelper.FioContentType.newFundsContent,
                    newFundsContentCipherBase64);
                Assert.AreEqual(newFundsContent, newFundsContentAlice);

                Dictionary<string, object> newFundsContentBob = await Fio.DHDecrypt(privateKeyBob,
                    publicKeyAlice,
                    Core.Helpers.FioHelper.FioContentType.newFundsContent,
                    newFundsContentCipherBase64);
                Assert.AreEqual(newFundsContent, newFundsContentBob);
            }
            catch (Exception e)
            {
                Assert.Fail("Expected no exception, but got: " + e);
            }
        }

        [Test]
        public async Task TestFioRequest()
        {
            try
            {
                var getInfoResult = await Fio.GetInfo();
                var getBlockResult = await Fio.GetBlock(getInfoResult.last_irreversible_block_num.ToString());

                byte[] payerPrivKey = CryptoHelper.GetPrivateKeyBytesWithoutCheckSum("5JRC9Kwmeo6CLPKx7TsJ4UybApmLcfxSJKgSMQi3QNtAh88r4Zu");
                byte[] payeePubKey = CryptoHelper.GetPublicKeyBytesWithoutCheckSum("FIO589xEUThDc1UfsXSyEg69vDj5sFviutdwLf5mR7xrJJaEj882y");
                //Console.WriteLine(payerPrivKey.Length);
                //Console.WriteLine(payeePubKey.Length);

                Dictionary<string, object> content = new Dictionary<string, object>() {
                    { "payee_public_address", "FIO589xEUThDc1UfsXSyEg69vDj5sFviutdwLf5mR7xrJJaEj882y" },
                    { "amount", "1" },
                    { "chain_code", "FIO" },
                    { "token_code", "FIO" },
                    { "memo", "Swag" },
                    { "hash", null },
                    { "offline_url", null }
                };

                string contentEncrypted = await Fio.DHEncrypt(payerPrivKey,
                    payeePubKey,
                    Core.Helpers.FioHelper.FioContentType.newFundsContent,
                    content);

                var trx = new Core.Api.v1.Transaction()
                {
                    //trx headers
                    expiration = getInfoResult.head_block_time.AddSeconds(60), //expire Seconds
                    ref_block_num = (UInt16)(getInfoResult.last_irreversible_block_num & 0xFFFF),
                    ref_block_prefix = getBlockResult.ref_block_prefix,
                    // trx info
                    max_net_usage_words = 0,
                    max_cpu_usage_ms = 0,
                    delay_sec = 0,
                    context_free_actions = new List<Core.Api.v1.Action>(),
                    transaction_extensions = new List<Extension>(),
                    actions = new List<Core.Api.v1.Action>()
                    {
                        new Core.Api.v1.Action()
                        {
                            account = "fio.reqobt",
                            name = "newfundsreq",
                            authorization = new List<PermissionLevel>()
                            {
                                new PermissionLevel() {actor = "bepto5rwh5gy", permission = "active" }
                            },
                            data = new Dictionary<string, object>() {
                                { "actor", "bepto5rwh5gy" },
                                { "payer_fio_address", "swag2@fiotestnet" },
                                { "payee_fio_address", "swag1@fiotestnet" },
                                { "max_fee", 1000000000000 },
                                { "tpid", "" },
                                { "content", contentEncrypted }
                            }
                        }
                    }
                };

                //var requiredKeys = new List<string>() { "FIO5g9NUxPpbiupHicH4ZMjqaLYLAghbARXMvJMkZv5uPx4MAbnZg" };
                string response = await Fio.CreateTransaction(trx);//, requiredKeys);
                Console.WriteLine(response);
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }

            Assert.IsTrue(true);
        }

        //[Test]
        //public async Task GetBlock()
        //{
        //    bool success = false;
        //    try
        //    {
        //        await EosUnitTestCases.GetBlock();
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(JsonConvert.SerializeObject(ex));
        //    }

        //    if (success)
        //        Console.WriteLine("Test GetBlock run successfuly.");
        //    else
        //        Console.WriteLine("Test GetBlock run failed.");
        //}

        //[Test]
        //public async Task GetProducers()
        //{
        //    bool success = false;
        //    try
        //    {
        //        await EosUnitTestCases.GetProducers();
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(JsonConvert.SerializeObject(ex));
        //    }

        //    if (success)
        //        Console.WriteLine("Test GetProducers run successfuly.");
        //    else
        //        Console.WriteLine("Test GetProducers run failed.");
        //}


        //public async Task GetScheduledTransactions()
        //{
        //    bool success = false;
        //    try
        //    {
        //        await EosUnitTestCases.GetScheduledTransactions();
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(JsonConvert.SerializeObject(ex));
        //    }

        //    if (success)
        //        Console.WriteLine("Test GetScheduledTransactions run successfuly.");
        //    else
        //        Console.WriteLine("Test GetScheduledTransactions run failed.");
        //}
        //public async Task CreateTransactionArrayData()
        //{
        //    bool success = false;
        //    try
        //    {
        //        await EosUnitTestCases.CreateTransactionArrayData();
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(JsonConvert.SerializeObject(ex));
        //    }

        //    if (success)
        //        Console.WriteLine("Test CreateTransactionArrayData run successfuly.");
        //    else
        //        Console.WriteLine("Test CreateTransactionArrayData run failed.");
        //}
        //public async Task CreateTransactionActionArrayStructData()
        //{
        //    bool success = false;
        //    try
        //    {
        //        await EosUnitTestCases.CreateTransactionActionArrayStructData();
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(JsonConvert.SerializeObject(ex));
        //    }

        //    if (success)
        //        Console.WriteLine("Test CreateTransactionActionArrayStructData run successfuly.");
        //    else
        //        Console.WriteLine("Test CreateTransactionActionArrayStructData run failed.");
        //}
        //public async Task CreateTransactionAnonymousObjectData()
        //{
        //    bool success = false;
        //    try
        //    {
        //        await EosUnitTestCases.CreateTransactionAnonymousObjectData();
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(JsonConvert.SerializeObject(ex));
        //    }

        //    if (success)
        //        Console.WriteLine("Test CreateTransactionAnonymousObjectData run successfuly.");
        //    else
        //        Console.WriteLine("Test CreateTransactionAnonymousObjectData run failed.");
        //}
        //public async Task CreateTransaction()
        //{
        //    bool success = false;
        //    try
        //    {
        //        await EosUnitTestCases.CreateTransaction();
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(JsonConvert.SerializeObject(ex));
        //    }

        //    if (success)
        //        Console.WriteLine("Test CreateTransaction run successfuly.");
        //    else
        //        Console.WriteLine("Test CreateTransaction run failed.");
        //}
        //public async Task CreateNewAccount()
        //{
        //    bool success = false;
        //    try
        //    {
        //        await EosUnitTestCases.CreateNewAccount();
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(JsonConvert.SerializeObject(ex));
        //    }

        //    if (success)
        //        Console.WriteLine("Test CreateNewAccount run successfuly.");
        //    else
        //        Console.WriteLine("Test CreateNewAccount run failed.");
        //}
        //public async Task CreateTransaction2Providers()
        //{
        //    bool success = false;
        //    try
        //    {
        //        await EosUnitTestCases.CreateTransaction2Providers();
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(JsonConvert.SerializeObject(ex));
        //    }

        //    if (success)
        //        Console.WriteLine("Test CreateTransaction2Providers run successfuly.");
        //    else
        //        Console.WriteLine("Test CreateTransaction2Providers run failed.");
        //}
    }
}
