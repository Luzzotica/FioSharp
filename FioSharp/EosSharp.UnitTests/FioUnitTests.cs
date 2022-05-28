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
using static EosSharp.Core.Helpers.FioHelper;

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

        [Test]
        public async Task TestDHEncrypt()
        {
            try { 
                string cipherAliceBase64 = await Fio.DHEncrypt(privateKeyAlice,
                    publicKeyBob,
                    FioContentType.newFundsContent,
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
