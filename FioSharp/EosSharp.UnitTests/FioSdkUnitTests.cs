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
    public class FioSdkIntegrationTests
    {
        FioSdk fioFaucet;
        FioSdk fioSdkKnown;

        const string testnetUrl = "http://52.40.41.71:8889";
        const string faucetPrivKey = "5KF2B21xT5pE5G3LNA6LKJc6AP2pAd2EnfpAUrJH12SFV8NtvCD";
        const string knownPrivKey = "5JRC9Kwmeo6CLPKx7TsJ4UybApmLcfxSJKgSMQi3QNtAh88r4Zu";
        const string knownPublicKey = "FIO5g9NUxPpbiupHicH4ZMjqaLYLAghbARXMvJMkZv5uPx4MAbnZg";
        const string knownActor = "bepto5rwh5gy";

        long defaultFee = FioSdk.AmountToSUF(80);

        string newFioDomain = "";
        string newFioAddress = "";

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

        private string GenerateTestingFioAddress(string domain)
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

        [SetUp]
        public async Task Setup()
        {
            fioFaucet = new FioSdk(
                faucetPrivKey,
                baseUrl: testnetUrl
                //test: true
            );
            fioSdkKnown = new FioSdk(
                knownPrivKey,
                baseUrl: testnetUrl
                //test: true
            );
            await fioFaucet.Init();
            await fioSdkKnown.Init();

            newFioDomain = GenerateTestingFioDomain();
            newFioAddress = GenerateTestingFioAddress(newFioDomain);
        }

        [Test]
        public void ConfirmGetActor()
        {
            Assert.AreEqual(knownActor, fioSdkKnown.GetActor());
        }

        [Test]
        public async Task ErrorWithLowMaxFee()
        {
            try
            {
                await fioFaucet.PushTransaction(new trnsfiopubky(
                    fioSdkKnown.GetPublicKey(),
                    FioSdk.AmountToSUF(10),
                    defaultFee,
                    "",
                    fioFaucet.GetActor()));
                Assert.Fail("Low max fee didn't throw an error");
            }
            catch (ApiErrorException e)
            {
                Assert.AreEqual(1, e.fields.Count);
                Assert.AreEqual("max_fee", e.fields[0].name);
                Assert.AreEqual("Fee exceeds supplied maximum.", e.fields[0].error);
            }
        }

        [Test]
        public void FioKeyGeneration()
        {
            string privateKeyExample = "5Kbb37EAqQgZ9vWUHoPiC2uXYhyGSFNbL6oiDp24Ea1ADxV1qnu";
            string publicKeyExample = "FIO5kJKNHwctcfUM5XZyiWSqSTM5HTzznJP9F3ZdbhaQAHEVq575o";

            // Generate public address
            string testMnemonic = "valley alien library bread worry brother bundle hammer loyal barely dune brave";
            MnemonicKey privateKeyRes = FioSdk.CreatePrivateKeyMnemonic(testMnemonic);
            Assert.AreEqual(privateKeyExample, privateKeyRes.fioPrivateKey);
            string publicKeyRes = FioSdk.DerivePublicKey(privateKeyRes.fioPrivateKey);
            Assert.AreEqual(publicKeyExample, publicKeyRes);
        }

        [Test]
        public void FioSUFUtilities()
        {
            long suf = FioSdk.AmountToSUF(100);
            Assert.AreEqual(suf, 100000000000);
            suf = FioSdk.AmountToSUF(500);
            Assert.AreEqual(suf, 500000000000);
            suf = FioSdk.AmountToSUF(506);
            Assert.AreEqual(suf, 506000000000);
            suf = FioSdk.AmountToSUF(1);
            Assert.AreEqual(suf, 1000000000);
            suf = FioSdk.AmountToSUF(2);
            Assert.AreEqual(suf, 2000000000);
            suf = FioSdk.AmountToSUF(2.568);
            Assert.AreEqual(suf, 2568000000);
            suf = FioSdk.AmountToSUF(2.123);
            Assert.AreEqual(suf, 2123000000);

            double amount = FioSdk.SUFToAmount(100000000000);
            Assert.AreEqual(amount, 100);
            amount = FioSdk.SUFToAmount(500000000000);
            Assert.AreEqual(amount, 500);
            amount = FioSdk.SUFToAmount(506000000000);
            Assert.AreEqual(amount, 506);
            amount = FioSdk.SUFToAmount(1000000000);
            Assert.AreEqual(amount, 1);
            amount = FioSdk.SUFToAmount(2000000000);
            Assert.AreEqual(amount, 2);
            amount = FioSdk.SUFToAmount(2568000000);
            Assert.AreEqual(amount, 2.568);
            amount = FioSdk.SUFToAmount(2123000000);
            Assert.AreEqual(amount, 2.123);
        }

        [Test]
        public void Validation()
        {
            Assert.IsFalse(FioSdk.IsChainCodeValid("$%34"));
            Assert.IsFalse(FioSdk.IsTokenCodeValid(""));
            Assert.IsFalse(FioSdk.IsFioAddressValid("f"));
            Assert.IsFalse(FioSdk.IsFioDomainValid("$%FG%"));
            Assert.IsFalse(FioSdk.IsFioPublicKeyValid("dfsd"));
            Assert.IsFalse(FioSdk.IsPublicAddressValid(""));

            Assert.IsTrue(FioSdk.IsChainCodeValid("FIO"));
            Assert.IsTrue(FioSdk.IsTokenCodeValid("FIO"));
            Assert.IsTrue(FioSdk.IsFioAddressValid("f@2"));
            Assert.IsTrue(FioSdk.IsFioAddressValid(newFioAddress));
            Assert.IsTrue(FioSdk.IsFioDomainValid(newFioDomain));
            Assert.IsTrue(FioSdk.IsFioPublicKeyValid(knownPublicKey));
            Assert.IsTrue(FioSdk.IsPublicAddressValid(knownPublicKey));
        }

        [Test]
        public void GetFioPublicKey()
        {
            Assert.AreEqual(knownPublicKey, fioSdkKnown.GetPublicKey());
        }
    }
}
