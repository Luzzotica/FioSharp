using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FioSharp;
using FioSharp.Core;
using FioSharp.Core.Api.v1;
using FioSharp.Core.Exceptions;
using FioSharp.Core.Helpers;
using FioSharp.Core.Providers;
using FioSharp.Models;
using FioSharp.Secp256k1;
using NBitcoin;
using NBitcoin.Crypto;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FioSharp.UnitTests
{
    public class FioSdkTests
    {
        FioSdk fioFaucet;
        FioSdk fioSdk1;
        FioSdk fioSdk2;

        const string devnetUrl = "http://52.40.41.71:8889";
        const string faucetPrivKey = "5KF2B21xT5pE5G3LNA6LKJc6AP2pAd2EnfpAUrJH12SFV8NtvCD";

        const int defaultWait = 500;

        long defaultFee = FioSdk.AmountToSUF(1000);
        long defaultFundAmount = FioSdk.AmountToSUF(1000);


        string testDomain;

        private string GetDateTimeNowMillis()
        {
            return ((long)DateTime.Now.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds).ToString();
        }

        private string GenerateTestingFioDomain()
        {
            return "testing-domain-" + GetDateTimeNowMillis();
        }

        private string GenerateTestingFioAddress(string domain = "fiotestnet")
        {
            return "testing" + GetDateTimeNowMillis() + "@" + domain;
        }

        private string GenerateObtId()
        {
            return GetDateTimeNowMillis();
        }

        private string GenerateHashForNft()
        {
            string now = GetDateTimeNowMillis().Substring(0, 13);
            return "f83b5702557b1ee76d966c6bf92ae0d038cd176aaf36f86a18e" + now;
        }

        /// <summary>
        /// * Generate a key pair for sender and receiver
        /// </summary>
        [SetUp]
        public async Task Setup()
        {
            fioFaucet = new FioSdk(
                faucetPrivKey,
                devnetUrl
            );
            await fioFaucet.Init();

            // Generate a new key for both of these bad boys
            string privKey1 = FioSdk.CreatePrivateKey().fioPrivateKey;
            string privKey2 = FioSdk.CreatePrivateKey().fioPrivateKey;
            Console.WriteLine("Priv Key 1: " + privKey1);
            Console.WriteLine("Priv Key 2: " + privKey2);
            fioSdk1 = new FioSdk(
                privKey1,
                devnetUrl
            );
            fioSdk2 = new FioSdk(
                privKey2,
                devnetUrl
            );
            await fioSdk1.Init();
            await fioSdk2.Init();

            // Create a domain for our own use
            //testDomain = GenerateTestingFioDomain();
            //await fioFaucet.PushTransaction(new regdomain(testDomain, fioFaucet.GetPublicKey(), defaultFee, "", fioFaucet.GetActor()));
        }

        /// <summary>
        /// * Send FIO to Public Key with {{trnsfiopubky}}
        /// * Confirm {{get_account}}
        /// * Confirm {{get_fio_balance}}
        /// * Register address with {{regaddress}}
        /// * Map address with {{addaddress}}
        /// * Confirm {{/get_pub_address}}
        /// * Confirm {{/get_pub_addresses}}
        /// * Remove address with {{remaddress}}
        /// * Confirm removal with {{/get_pub_address}}
        /// * Add bundles with {{addbundles}}
        /// * Confirm bundle change with {{/get_fio_names}}  
        /// </summary>
        [Test]
        public async Task FioSendOBTDataAndBundles()
        {
            string fioAddress = GenerateTestingFioAddress();
            await RegisterFioHandleToSdk(fioAddress, fioSdk1);

            // Remove address and confirm it
            try
            {
                await fioSdk1.PushTransaction(new remaddress(
                        fioAddress,
                        new List<object> { new Dictionary<string, object>{
                        { "chain_code", "FIO" },
                        { "token_code", "FIO" },
                        { "public_address", fioSdk1.GetPublicKey() }
                    } },
                        defaultFee,
                        "",
                        fioSdk1.GetActor()));

                // Wait for block to confirm
                await Task.Delay(defaultWait);
            }
            catch (ApiErrorException e)
            {
                Assert.Fail(e.ToString() + "\n" + e.StackTrace);
            }

            try
            {
                GetPubAddressResponse getPubAddress = await fioSdk1.GetFioApi().GetPubAddress(fioAddress, "FIO", "FIO");
            }
            catch (ApiErrorException e)
            {
                Assert.AreEqual(e.message, "Public address not found");
            }

            // Add Bundles and confirm they exist
            try
            {
                GetFioNamesResponse resp = await fioSdk1.GetFioNames();
                int bundledTxns = resp.fio_addresses[0].remaining_bundled_tx;
                Assert.Greater(bundledTxns, 0);

                await fioSdk1.PushTransaction(new addbundles(
                        fioAddress,
                        1,
                        defaultFee,
                        "",
                        fioSdk1.GetActor()));

                // Wait for block to confirm
                await Task.Delay(defaultWait);

                resp = await fioSdk1.GetFioNames();
                int newBundledTxns = resp.fio_addresses[0].remaining_bundled_tx;
                Assert.Greater(newBundledTxns, bundledTxns);
            }
            catch (ApiErrorException e)
            {
                Assert.Fail(e.ToString() + "\n" + e.StackTrace);
            }
        }

        /// <summary>
        /// * Generate two key pairs
        /// * Register FIO Crypto Handle to public key #1
        /// * Confirm with {{/get_fio_addresses}}
        ///     and {{/avail_check}}
        /// * Register FIO Crypto Handle to public key #2
        /// * Register domain with {{regdomain}} for public key #1
        ///     and use the new crypto handle for #2 in the TPID field
        /// * Confirm with {{/get_fio_names}}
        ///     and {{/get_fio_domains}}
        ///     and {{/avail_check}}
        /// * Confirm balance for #2 account changed because of fee paid to tpid
        /// * Make domain public with {{setdomainpub}}
        /// * Transfer domain with {{xferdomain}}
        /// </summary>
        [Test]
        public async Task TPIDCryptoHandlesAndDomains()
        {
            // Register and map an address, confirm it exists
            string fioAddress = GenerateTestingFioAddress();
            await RegisterFioHandleToSdk(fioAddress, fioSdk1);
            string fioAddress2 = GenerateTestingFioAddress();
            await RegisterFioHandleToSdk(fioAddress2, fioSdk2);

            // Test TPID
            string fioDomain = GenerateTestingFioDomain();
            try
            {
                // Get balance for key 2, compare against later
                GetFioBalanceResponse getFioBalance = await fioSdk2.GetFioBalance();
                ulong currBalance = getFioBalance.balance;

                // Create domain
                await fioSdk1.PushTransaction(new regdomain(
                    fioDomain,
                    fioSdk1.GetPublicKey(),
                    defaultFee,
                    fioAddress2,
                    fioSdk1.GetActor()));

                // Wait for block to confirm
                await Task.Delay(defaultWait);

                // Check domain was registered: fio names, avail check, fio domains
                GetFioNamesResponse resp = await fioSdk1.GetFioNames();
                Assert.AreEqual(resp.fio_domains[0].fio_domain, fioDomain);
                GetFioDomainsResponse resp2 = await fioSdk1.GetFioDomains(1, 0);
                Assert.AreEqual(resp2.fio_domains[0].fio_domain, fioDomain);
                AvailCheckResponse availCheck = await fioSdk1.GetFioApi().AvailCheck(fioDomain);
                Assert.AreEqual(availCheck.is_registered, 1);

                // Claim funds from transactions
                await fioSdk2.PushTransaction(new tpidclaim(fioSdk2.GetActor()));

                // Wait for block to confirm
                await Task.Delay(defaultWait);

                getFioBalance = await fioSdk2.GetFioBalance();
                ulong newBalance = getFioBalance.balance;
                Assert.Greater(newBalance, currBalance);
            }
            catch (ApiErrorException e)
            {
                Assert.Fail(e.ToString() + "\n" + e.StackTrace);
            }

            // Make domain public and transfer it
            try
            {
                // Register domain
                await fioSdk1.PushTransaction(new setdomainpub(
                    fioDomain,
                    1, 
                    defaultFee,
                    fioAddress2,
                    fioSdk1.GetActor()));

                // Wait for block to confirm
                await Task.Delay(defaultWait);

                // Check domain is public
                GetFioDomainsResponse resp2 = await fioSdk1.GetFioDomains(1, 0);
                Assert.AreEqual(resp2.fio_domains[0].fio_domain, fioDomain);
                Assert.AreEqual(resp2.fio_domains[0].is_public, 1);

                // Transfer domain
                await fioSdk1.PushTransaction(new xferdomain(
                    fioDomain,
                    fioSdk2.GetPublicKey(),
                    defaultFee,
                    fioAddress2,
                    fioSdk1.GetActor()));

                // Wait for block to confirm
                await Task.Delay(defaultWait);

                // Check domain was transfered
                GetFioNamesResponse resp = await fioSdk1.GetFioNames();
                Assert.AreEqual(resp.fio_domains.Count, 0);
                resp = await fioSdk2.GetFioNames();
                Assert.AreEqual(resp.fio_domains[0].fio_domain, fioDomain);
            }
            catch (ApiErrorException e)
            {
                Assert.Fail(e.ToString() + "\n" + e.StackTrace);
            }
        }

        /// <summary>
        /// * Create two users and register crypto handles for the users
        /// * Request FIO with {{newfundsreq}}
        /// * Confirm {{/get_sent_fio_requests}}
        ///     {{/get_received_fio_requests}}
        ///     {{/get_pending_fio_requests}}
        ///     {{/get_cancelled_fio_requests}}
        /// * Confirm status with {{/get_obt_data}}
        /// * Confirm encrypted content field is properly decrypted by both user keys
        /// * Respond to request and include {{recordobt}} with the {{fio_request_id}}
        /// * Confirm { {/ get_sent_fio_requests}}
        ///     {{/get_received_fio_requests}}
        ///     {{/get_pending_fio_requests}}
        ///     {{/get_cancelled_fio_requests}}
        /// * Confirm status with {{/get_obt_data}}
        /// * Create new request
        /// * Cancel request
        /// * Confirm {{/get_sent_fio_requests}}
        ///     {{/get_received_fio_requests}}
        ///     {{/get_pending_fio_requests}}
        ///     {{/get_cancelled_fio_requests}}
        /// </summary>
        [Test]
        public async Task FioRequest()
        {
            // Register and map an address, confirm it exists
            string fioAddress = GenerateTestingFioAddress();
            await RegisterFioHandleToSdk(fioAddress, fioSdk1);
            string fioAddress2 = GenerateTestingFioAddress();
            await RegisterFioHandleToSdk(fioAddress2, fioSdk2);

            // Request fio
            try
            {
                Dictionary<string, object> content = new newfundsreq_content(
                    fioSdk1.GetPublicKey(),
                    "1",
                    "FIO",
                    "FIO",
                    "Swag",
                    null, null).ToJsonObject();
                string encryptedContent = await fioSdk1.DHEncrypt(
                    fioSdk2.GetPublicKey(),
                    FioHelper.NEW_FUNDS_CONTENT,
                    content);

                await fioSdk1.PushTransaction(new newfundsreq(
                    fioAddress2,
                    fioAddress,
                    "",
                    encryptedContent,
                    defaultFee,
                    fioSdk1.GetActor()));

                // Confirm request was sent, received, and nothing was cancelled
                GetSentFioRequestsResponse getSentFioRequests = await fioSdk1.GetSentFioRequests(1, 0);
                Assert.Greater(getSentFioRequests.requests.Count, 0);
                GetReceivedFioRequestsResponse getReceivedFioRequests = await fioSdk2.GetReceivedFioRequests(1, 0);
                Assert.Greater(getReceivedFioRequests.requests.Count, 0);
                GetPendingFioRequestsResponse getPendingFioRequests = await fioSdk1.GetPendingFioRequests(1, 0);
                Assert.AreEqual(getPendingFioRequests.requests.Count, 0);
                GetCancelledFioRequestsResponse getCancelledFioRequests = await fioSdk1.GetCancelledFioRequests(1, 0);
                Assert.AreEqual(getCancelledFioRequests.requests.Count, 0);
            }
            catch (ApiErrorException e)
            {
                Assert.Fail(e.ToString() + "\n" + e.StackTrace);
            }
        }

        //[Test]
        //public async Task OBTData()
        //{

        //}

        //[Test]
        //public async Task SignedNFTs()
        //{

        //}

        //[Test]
        //public async Task Staking()
        //{

        //

        public async Task RegisterFioHandleToSdk(string handle, FioSdk sdk, bool fund = true)
        {
            try
            {
                if (fund)
                {
                    // Fund the new account
                    await fioFaucet.PushTransaction(new trnsfiopubky(
                        sdk.GetPublicKey(),
                        defaultFundAmount,
                        defaultFee,
                        "",
                        fioFaucet.GetActor()));

                    GetFioBalanceResponse getFioBalance = await sdk.GetFioBalance();
                    Assert.AreEqual(getFioBalance.balance, defaultFundAmount);
                }

                // Register and map
                await sdk.PushTransaction(new regaddress(
                    handle,
                    sdk.GetPublicKey(),
                    defaultFee,
                    "",
                    sdk.GetActor()));
                await Task.Delay(defaultWait);
                await sdk.PushTransaction(new addaddress(
                    handle,
                    new List<object> { new Dictionary<string, object>{
                        { "chain_code", "FIO" },
                        { "token_code", "FIO" },
                        { "public_address", sdk.GetPublicKey() }
                    } },
                    defaultFee,
                    "",
                    sdk.GetActor()));

                // Wait for block to confirm
                await Task.Delay(defaultWait);

                // Check address exists, and that the key is correct
                GetFioAddressesResponse getFioAddresses = await sdk.GetFioAddresses(1, 0);
                Assert.Greater(getFioAddresses.fio_addresses.Count, 0);
                Assert.AreEqual(getFioAddresses.fio_addresses[0].fio_address, handle);
                GetPubAddressResponse getPubAddress = await sdk.GetFioApi().GetPubAddress(handle, "FIO", "FIO");
                Assert.AreEqual(getPubAddress.public_address, sdk.GetPublicKey());
                GetPubAddressesResponse getPubAddresses = await sdk.GetFioApi().GetPubAddresses(handle, 1, 0);
                Assert.AreEqual(getPubAddresses.public_addresses[0].public_address, sdk.GetPublicKey());
                AvailCheckResponse availCheck = await sdk.GetFioApi().AvailCheck(handle);
                Assert.AreEqual(availCheck.is_registered, 1);
            }
            catch (ApiErrorException e)
            {
                Assert.Fail(e.ToString() + "\n" + e.StackTrace);
            }
        }
    }
}
