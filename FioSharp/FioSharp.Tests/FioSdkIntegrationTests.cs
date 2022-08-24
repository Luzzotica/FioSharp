using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FioSharp.Core.Api.v1;
using FioSharp.Core.Exceptions;
using FioSharp.Core.Helpers;
using FioSharp.Tests.Core;
using NUnit.Framework;

namespace FioSharp.UnitTests
{
    public class FioSdkTests
    {
        FioSdkIntegrationTestCases testCases;

        /// <summary>
        /// * Generate a key pair for sender and receiver
        /// </summary>
        [SetUp]
        public async Task Setup()
        {
            testCases = new FioSdkIntegrationTestCases();
            await testCases.Setup();
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
            await testCases.FioSendOBTDataAndBundles();
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
            await testCases.TPIDCryptoHandlesAndDomains();
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
        /// * Fulfill the request
        /// * Create {{recordobt}} with the {{fio_request_id}}
        /// * Confirm status with {{/get_obt_data}}
        /// * Confirm {{/ get_sent_fio_requests}}
        ///     {{/get_received_fio_requests}}
        ///     {{/get_pending_fio_requests}}
        ///     {{/get_cancelled_fio_requests}}
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
            await testCases.FioRequest();
        }

        /// <summary>
        /// * Create two users and register crypto handles for the users
        /// * Send FIO from user1 to user2 with {{trnsfiopubky}}
        /// * Add transaction metadata with {{recordobt}}
        /// * Confirm status with {{/get_obt_data}}
        /// * Confirm encrypted content field is properly decrypted by both user keys  
        /// </summary>
        [Test]
        public async Task OBTData()
        {
            await testCases.OBTData();
        }

        /// <summary>
        /// * Create fio handle
        /// * Create signature with {{addnft}}
        /// * Confirm with {{/get_nfts_fio_address}}
        ///     {{/get_nfts_contract}}
        ///     {{/get_nfts_hash}}
        /// * Remove signature with {{remnft}}
        /// * Confirm with {{/get_nfts_fio_address}}
        ///     {{/get_nfts_contract}}
        ///     {{/get_nfts_hash}}
        /// </summary>
        [Test]
        public async Task SignedNFTs()
        {
            await testCases.SignedNFTs();
        }

        /// <summary>
        /// * Register fio handle
        /// * Vote on a BP
        /// * Stake FIO tokens with {{stakefio}}
        /// * Confirm with {{get_fio_balance}}
        /// * Unstake portion of staked FIO with {{unstakefio}}
        /// * Confirm with {{get_fio_balance}}
        /// </summary>
        [Test]
        public async Task Staking()
        {
            await testCases.Staking();
        }
    }
}
