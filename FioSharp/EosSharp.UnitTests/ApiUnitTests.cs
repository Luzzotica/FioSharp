using NUnit.Framework;
using EosSharp.Core;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Providers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace EosSharp.UnitTests
{
    public class Tests
    {
        
        ApiUnitTestCases ApiUnitTestCases;

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

                HttpEndpoint = "https://api.fiotest.alohaeos.com:443",
                ChainId = "b20901380af44ef59c5918439a1f9a41d83669020319a80574b804a5f95cbd7e" // Testnet

                //HttpEndpoint = "http://localhost:8888",
                //ChainId = "cf057bbfb72640471fd910bcb67639c22df9f92470936cddc1ade0e2f2e7dc4f"
            };
            var eosApi = new EosApi(eosConfig, new HttpHandler());

            ApiUnitTestCases = new ApiUnitTestCases(eosConfig, eosApi);
        }


        [Test]
        public async Task GetInfo()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetInfo();
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
                await ApiUnitTestCases.GetAccount();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetCode()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetCode();
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
                GetAbiResponse resp = await ApiUnitTestCases.GetReqObtAbi();
                //foreach (AbiStruct s in resp.abi.structs)
                //{
                //    Console.WriteLine(s.name);
                //    Console.WriteLine(s.@base);
                //    foreach (AbiField f in s.fields)
                //    {
                //        Console.Write(f.type);
                //        Console.Write(" ");
                //        Console.Write(f.name);
                //        Console.Write(" | ");
                //    }
                //    Console.WriteLine();
                //}
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetRawCodeAndAbi()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetRawCodeAndAbi();
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
                await ApiUnitTestCases.GetRawAbi();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task AbiJsonToBin()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.AbiJsonToBin();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task AbiBinToJson()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.AbiBinToJson();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetRequiredKeys()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetRequiredKeys();
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
                await ApiUnitTestCases.GetBlock();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetBlockHeaderState()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetBlockHeaderState();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetTableRows()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetTableRows();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetTableByScope()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetTableByScope();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetCurrencyBalance()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetCurrencyBalance();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetCurrencyStats()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetCurrencyStats();
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
                await ApiUnitTestCases.GetProducers();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetProducerSchedule()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetProducerSchedule();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetScheduledTransactions()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetScheduledTransactions();
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
                PushTransactionResponse resp = await ApiUnitTestCases.PushTransaction();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
        //[Test]
        //public async Task GetControlledAccounts()
        //{
        //    bool success = false;
        //    try
        //    {
        //        await ApiUnitTestCases.GetControlledAccounts();
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(JsonConvert.SerializeObject(ex));
        //    }

        //    Assert.IsTrue(success);
        //}

    }
}

