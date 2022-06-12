using NUnit.Framework;
using FioSharp.Core;
using FioSharp.Core.Api.v1;
using FioSharp.Core.Providers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using FioSharp.Core.Helpers;
using System.Linq;
using FioSharp.Core.Exceptions;

namespace FioSharp.UnitTests
{
    public class Tests
    {
        FioApi api;
        const string privateKey1 = "5JRC9Kwmeo6CLPKx7TsJ4UybApmLcfxSJKgSMQi3QNtAh88r4Zu";
        const string publicKey1 = "FIO5g9NUxPpbiupHicH4ZMjqaLYLAghbARXMvJMkZv5uPx4MAbnZg";
        const string privateKey2 = "5KdDvp6ExuAPauN65HKKihVuDd28pTRmHyM8iooz8CBeW48w1Pt";
        const string publicKey2 = "FIO589xEUThDc1UfsXSyEg69vDj5sFviutdwLf5mR7xrJJaEj882y";
        const string fioAddress1 = "swag1@fiotestnet";
        const string fioAddress2 = "swag2@fiotestnet";
        const string actor1 = "bepto5rwh5gy";
        const string actor2 = "zbu3hi2wwww1";

        [SetUp]
        public void Setup()
        {
            var eosConfig = new FioConfigurator()
            {
                SignProvider = new DefaultSignProvider(new List<string>() {
                    privateKey1,
                    privateKey2
                }),

                //HttpEndpoint = "https://nodes.eos42.io", //Mainnet
                //ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906"

                HttpEndpoint = "https://api.fiotest.alohaeos.com:443",
                ChainId = "b20901380af44ef59c5918439a1f9a41d83669020319a80574b804a5f95cbd7e" // Testnet

                //HttpEndpoint = "http://localhost:8888",
                //ChainId = "cf057bbfb72640471fd910bcb67639c22df9f92470936cddc1ade0e2f2e7dc4f"
            };
            api = new FioApi(eosConfig, new HttpHandler());
        }

        [Test]
        public async Task GetFioBalance()
        {
            bool success = false;
            try
            {
                GetFioBalanceResponse resp = await api.GetFioBalance(publicKey1);
                //Console.WriteLine(resp.balance);
                //Console.WriteLine(resp.available);
                //Console.WriteLine(resp.roe);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetFioNames()
        {
            bool success = false;
            try
            {
                await api.GetFioNames(publicKey1);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetFioAddresses()
        {
            bool success = false;
            try
            {
                await api.GetFioAddresses(publicKey1, 10, 0);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetFioDomains()
        {
            bool success = false;
            try
            {
                await api.GetFioDomains(publicKey1, 10, 0);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task AvailCheck()
        {
            bool success = false;
            try
            {
                await api.AvailCheck("swag1@tester");
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(JsonConvert.SerializeObject(e));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetPublicAddress()
        {
            bool success = false;
            try
            {
                await api.GetPublicAddress(fioAddress1, "FIO", "FIO");
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetPublicAddresses()
        {
            bool success = false;
            try
            {
                await api.GetPublicAddresses(fioAddress1, 10, 0);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetSentFioRequests()
        {
            bool success = false;
            try
            {
                await api.GetSentFioRequests(publicKey1, 10, 0);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetReceivedFioRequests()
        {
            bool success = false;
            try
            {
                await api.GetReceivedFioRequests(publicKey1, 10, 0);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetPendingFioRequests()
        {
            bool success = false;
            try
            {
                await api.GetPendingFioRequests(publicKey1, 10, 0);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetCancelledFioRequests()
        {
            bool success = false;
            try
            {
                await api.GetCancelledFioRequests(publicKey1, 10, 0);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetObtData()
        {
            bool success = false;
            try
            {
                await api.GetObtData(publicKey1, 10, 0);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetLocks()
        {
            bool success = false;
            try
            {
                await api.GetLocks(publicKey1);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetFee()
        {
            bool success = false;
            try
            {
                await api.GetFee(Accounts.FioAddress.ADDADDRESS, fioAddress1);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetNftsFioAddress()
        {
            bool success = false;
            try
            {
                await api.GetNftsFioAddress(fioAddress1, 10, 0);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetNftsContract()
        {
            bool success = false;
            try
            {
                await api.GetNftsContract("", "", "", 10, 0);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetNftsHash()
        {
            bool success = false;
            try
            {
                await api.GetNftsHash("", 10, 0);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetActor()
        {
            bool success = false;
            try
            {
                string actor = (await api.GetActor(publicKey1)).actor;
                Assert.AreEqual(actor1, actor);
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetAccount()
        {
            bool success = false;
            try
            {
                await api.GetAccount("fio.token");
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetInfo()
        {
            bool success = false;
            try
            {
                await api.GetInfo();
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetBlock()
        {
            bool success = false;
            try
            {
                var getInfoResult = await api.GetInfo();
                var getBlockResult = await api.GetBlock(getInfoResult.last_irreversible_block_num.ToString());
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetAbi()
        {
            bool success = false;
            try
            {
                Abi resp = await api.GetAbi("fio.reqobt");
                foreach (AbiStruct s in resp.structs)
                {
                    Console.WriteLine();
                    Console.WriteLine(s.name);
                    foreach (AbiField f in s.fields)
                    {
                        Console.Write(f.type);
                        Console.Write(" ");
                        Console.Write(f.name);
                        Console.Write(" | ");
                    }
                    Console.WriteLine();
                }
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetRawAbi()
        {
            bool success = false;
            try
            {
                await api.GetRawAbi("fio.token");
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetProducers()
        {
            bool success = false;
            try
            {
                await api.GetProducers("10", "0");
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task PushTransaction()
        {
            bool success = false;
            try
            {
                await CreateTransaction();
                success = true;
            }
            catch (ApiErrorException ex)
            {
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        private async Task<PushTransactionResponse> CreateTransaction()
        {
            var getInfoResult = await api.GetInfo();
            var getBlockResult = await api.GetBlock(getInfoResult.last_irreversible_block_num.ToString());

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
                context_free_actions = new List<Core.Api.v1.Action>(),
                transaction_extensions = new List<Extension>(),
                actions = new List<Core.Api.v1.Action>()
                {
                    new Core.Api.v1.Action()
                    {
                        account = "fio.token",
                        name = "trnsfiopubky",
                        authorization = new List<PermissionLevel>()
                        {
                            new PermissionLevel() {actor = "bepto5rwh5gy", permission = "active" }
                        },
                        data = new Dictionary<string, object>() {
                            { "actor", "bepto5rwh5gy" },
                            { "payee_public_key", "FIO589xEUThDc1UfsXSyEg69vDj5sFviutdwLf5mR7xrJJaEj882y" },
                            { "amount", 1000000000 },
                            { "max_fee", 1000000000000 },
                            { "tpid", "" },
                        }
                    }
                }
            };

            var abiSerializer = new AbiSerializationProvider(api);
            var packedTrx = await abiSerializer.SerializePackedTransaction(trx);
            var requiredKeys = new List<string>() { "FIO5g9NUxPpbiupHicH4ZMjqaLYLAghbARXMvJMkZv5uPx4MAbnZg" };
            var signatures = await api.Config.SignProvider.Sign(api.Config.ChainId, requiredKeys, packedTrx);

            return await api.PushTransaction(signatures.ToArray(), SerializationHelper.ByteArrayToHexString(packedTrx));
        }

    }
}

