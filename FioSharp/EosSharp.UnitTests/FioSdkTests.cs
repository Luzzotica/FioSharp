using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FioSharp;
using FioSharp.Core;
using FioSharp.Core.Api.v1;
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
        FioSdk fioSdk1;
        FioSdk fioSdk2;

        const string privKey1 = "5JRC9Kwmeo6CLPKx7TsJ4UybApmLcfxSJKgSMQi3QNtAh88r4Zu";
        const string privKey2 = "5KdDvp6ExuAPauN65HKKihVuDd28pTRmHyM8iooz8CBeW48w1Pt";
        const string publicKey1 = "FIO5g9NUxPpbiupHicH4ZMjqaLYLAghbARXMvJMkZv5uPx4MAbnZg";
        const string publicKey2 = "FIO589xEUThDc1UfsXSyEg69vDj5sFviutdwLf5mR7xrJJaEj882y";
        const string actor1 = "bepto5rwh5gy";
        const string actor2 = "zbu3hi2wwww1";
        string testFioAddress1 = "swag1@fiotestnet";
        string testFioAddress2 = "swag2@fiotestnet";

        long defaultFee = 800 * FioSdk.SUFUnit;

        string sender = "";
        string receiver = "";
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
            fioSdk1 = new FioSdk(
                "5JtUScZK2XEp3g9gh7F8bwtPTRAkASmNrrftmx4AxDKD5K4zDnr",
                test: true);
            fioSdk2 = new FioSdk(
                "5JtUScZK2XEp3g9gh7F8bwtPTRAkASmNrrftmx4AxDKD5K4zDnr",
                test: true);
            await fioSdk1.Init();
            await fioSdk2.Init();

            newFioDomain = GenerateTestingFioDomain();
            newFioAddress = GenerateTestingFioAddress(newFioDomain);
        }

        [Test]
        public void ConfirmGetActor()
        {
            Assert.AreEqual(actor1, fioSdk1.GetActor());
            Assert.AreEqual(actor2, fioSdk2.GetActor());
        }

        [Test]
        public void ErrorWithLowMaxFee()
        {

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
            Assert.IsTrue(FioSdk.IsFioPublicKeyValid(publicKey1));
            Assert.IsTrue(FioSdk.IsPublicAddressValid(publicKey1));
        }

        [Test]
        public void GetFioPublicKey()
        {
            Assert.AreEqual(publicKey1, fioSdk1.GetPublicKey());
        }

        [Test]
        public void GetFioBalance()
        {
            Assert.DoesNotThrow(async () => await fioSdk1.GetFioApi().GetFioBalance(fioSdk1.GetPublicKey()));
        }

        [Test]
        public void RegisterFioDomain()
        {
            //fioSdk1.PushTransaction
            Assert.DoesNotThrow(async () => await fioSdk1.GetFioApi().GetFioBalance(fioSdk1.GetPublicKey()));
        }

  //      it(`getFioBalance`, async () => {
  //          const result = await fioSdk.genericAction('getFioBalance', { })

  //  expect(result).to.have.all.keys('balance', 'available')
  //  expect(result.balance).to.be.a('number')
  //  expect(result.available).to.be.a('number')
  //})

  //it(`Register fio domain`, async () => {
  //          const result = await fioSdk.genericAction('registerFioDomain', { fioDomain: newFioDomain, maxFee: defaultFee })

  //  expect(result).to.have.all.keys('status', 'expiration', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.expiration).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`Renew fio domain`, async () => {
  //          const result = await fioSdk.genericAction('renewFioDomain', { fioDomain: newFioDomain, maxFee: defaultFee })

  //  expect(result).to.have.all.keys('status', 'expiration', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.expiration).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`setFioDomainVisibility true`, async () => {
  //          const result = await fioSdk.genericAction('setFioDomainVisibility', {
  //          fioDomain: newFioDomain,
  //    isPublic: true,
  //    maxFee: defaultFee,
  //    technologyProviderId: ''
  //          })

  //  expect(result).to.have.all.keys('status', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`Register fio address`, async () => {
  //          const result = await fioSdk.genericAction('registerFioAddress', {
  //          fioAddress: newFioAddress,
  //    maxFee: defaultFee
  //          })

  //  expect(result).to.have.all.keys('status', 'expiration', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.expiration).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`getFioNames`, async () => {
  //          const result = await fioSdk.genericAction('getFioNames', { fioPublicKey: publicKey })

  //  expect(result).to.have.all.keys('fio_domains', 'fio_addresses')
  //  expect(result.fio_domains).to.be.a('array')
  //  expect(result.fio_addresses).to.be.a('array')
  //})

  //it(`getFioDomains`, async () => {
  //          try
  //          {
  //              const result = await fioSdk.genericAction('getFioDomains', { fioPublicKey: fioSdk.publicKey })

  //    expect(result).to.have.all.keys('fio_domains', 'more')
  //            expect(result.fio_domains).to.be.a('array')
  //          }
  //          catch (e)
  //          {
  //              console.log(e);
  //          }
  //      })

  //it(`Register owner fio address`, async () => {
  //          const newFioAddress2 = generateTestingFioAddress(newFioDomain)
  //  const result = await fioSdk.genericAction('registerFioAddress', {
  //          fioAddress: newFioAddress2,
  //    ownerPublicKey: publicKey2,
  //    maxFee: defaultFee
  //  })
  //  expect(result).to.have.all.keys('status', 'expiration', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.expiration).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`setFioDomainVisibility false`, async () => {
  //          const result = await fioSdk.genericAction('setFioDomainVisibility', {
  //          fioDomain: newFioDomain,
  //    isPublic: false,
  //    maxFee: defaultFee,
  //    technologyProviderId: ''
  //          })

  //  expect(result).to.have.all.keys('status', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`setFioDomainVisibility true`, async () => {
  //          const result = await fioSdk.genericAction('setFioDomainVisibility', {
  //          fioDomain: newFioDomain,
  //    isPublic: true,
  //    maxFee: defaultFee,
  //    technologyProviderId: ''
  //          })

  //  expect(result).to.have.all.keys('status', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`getFee for transferFioDomain`, async () => {
  //          const result = await fioSdk.genericAction('getFeeForTransferFioDomain', {
  //          fioAddress: newFioAddress
  //          })

  //  expect(result).to.have.all.keys('fee')
  //  expect(result.fee).to.be.a('number')
  //})

  //it(`Transfer fio domain`, async () => {
  //          const result = await fioSdk.genericAction('transferFioDomain', {
  //          fioDomain: newFioDomain,
  //    newOwnerKey: pubKeyForTransfer,
  //    maxFee: defaultFee
  //          })

  //  expect(result).to.have.all.keys('status', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`getFee for addBundledTransactions`, async () => {
  //          const result = await fioSdk.genericAction('getFeeForAddBundledTransactions', {
  //          fioAddress: newFioAddress
  //          })

  //  expect(result).to.have.all.keys('fee')
  //  expect(result.fee).to.be.a('number')
  //})

  //it(`add Bundled Transactions`, async () => {
  //          const result = await fioSdk.genericAction('addBundledTransactions', {
  //          fioAddress: newFioAddress,
  //    bundleSets: defaultBundledSets,
  //    maxFee: defaultFee
  //          })

  //  expect(result).to.have.all.keys('status', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`Renew fio address`, async () => {
  //          const result = await fioSdk.genericAction('renewFioAddress', { fioAddress: newFioAddress, maxFee: defaultFee })

  //  expect(result).to.have.all.keys('status', 'expiration', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.expiration).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`Push Transaction - renewaddress`, async () => {
  //          await timeout(2000)
  //  const result = await fioSdk.genericAction('pushTransaction', {
  //          action: 'renewaddress',
  //    account: 'fio.address',
  //    data:
  //              {
  //              fio_address: newFioAddress,
  //      max_fee: defaultFee,
  //      tpid: ''
  //    }
  //          })

  //  expect(result).to.have.all.keys('status', 'expiration', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.expiration).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`getFee for addPublicAddress`, async () => {
  //          const result = await fioSdk.genericAction('getFeeForAddPublicAddress', {
  //          fioAddress: newFioAddress
  //          })

  //  expect(result).to.have.all.keys('fee')
  //  expect(result.fee).to.be.a('number')
  //})

  //it(`Add public address`, async() => {
  //  const result = await fioSdk.genericAction('addPublicAddress', {
  //    fioAddress: newFioAddress,
  //    chainCode: fioChainCode,
  //    tokenCode: fioTokenCode,
  //    publicAddress: '1PMycacnJaSqwwJqjawXBErnLsZ7RkXUAs',
  //    maxFee: defaultFee,
  //    technologyProviderId: ''
  //  })

  //  expect(result).to.have.all.keys('status', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`Add public addresses`, async () => {
  //    const result = await fioSdk.genericAction('addPublicAddresses', {
  //    fioAddress: newFioAddress,
  //    publicAddresses:
  //        [
  //      {
  //        chain_code: ethChainCode,
  //        token_code: ethTokenCode,
  //        public_address: 'xxxxxxyyyyyyzzzzzz',
  //      },
  //      {
  //        chain_code: fioChainCode,
  //        token_code: fioTokenCode,
  //        public_address: publicKey,
  //      }
  //    ],
  //    maxFee: defaultFee,
  //    technologyProviderId: ''
  //    })

  //  expect(result).to.have.all.keys('status', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})


  //it(`getPublicAddress`, async () => {
  //const result = await fioSdk.genericAction('getPublicAddress', {
  //fioAddress: newFioAddress, chainCode: fioChainCode, tokenCode: fioTokenCode
  //  })

  //  expect(result.public_address).to.be.a('string')
  //})


  //it(`getPublicAddresses`, async () => {
  //const result = await fioSdk.genericAction('getPublicAddresses', {
  //fioAddress: newFioAddress, limit: 10, offset: 0
  //  })

  //  expect(result).to.have.all.keys('public_addresses', 'more')
  //  expect(result.public_addresses).to.be.a('array')
  //  expect(result.more).to.be.a('boolean')
  //})

  //it(`getFee for removePublicAddresses`, async () => {
  //const result = await fioSdk.genericAction('getFeeForRemovePublicAddresses', {
  //fioAddress: newFioAddress
  //  })

  //  expect(result).to.have.all.keys('fee')
  //  expect(result.fee).to.be.a('number')
  //})

  //it(`Remove public addresses`, async () => {

  //    const result = await fioSdk.genericAction('removePublicAddresses', {
  //    fioAddress: newFioAddress,
  //    publicAddresses:
  //        [
  //      {
  //        chain_code: ethChainCode,
  //        token_code: ethTokenCode,
  //        public_address: 'xxxxxxyyyyyyzzzzzz',
  //      }
  //    ],
  //    maxFee: defaultFee,
  //    technologyProviderId: ''
  //    })
  //  expect(result).to.have.all.keys('status', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`getFee for removeAllPublicAddresses`, async () => {

  //    const result = await fioSdk.genericAction('getFeeForRemoveAllPublicAddresses', {
  //    fioAddress: newFioAddress
  //    })

  //  expect(result).to.have.all.keys('fee')
  //  expect(result.fee).to.be.a('number')
  //})

  //it(`Remove all public addresses`, async () => {
  //    await fioSdk.genericAction('addPublicAddresses', {
  //    fioAddress: newFioAddress,
  //    publicAddresses:
  //        [
  //      {
  //        chain_code: ethChainCode,
  //        token_code: ethTokenCode,
  //        public_address: 'xxxxxxyyyyyyzzzzzz1',
  //      }
  //    ],
  //    maxFee: defaultFee,
  //    technologyProviderId: ''
  //    })

  //  const result = await fioSdk.genericAction('removeAllPublicAddresses', {
  //    fioAddress: newFioAddress,
  //    maxFee: defaultFee,
  //    technologyProviderId: ''
  //  })
  //  expect(result).to.have.all.keys('status', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})

  //it(`isAvailable true`, async () => {
  //const result = await fioSdk.genericAction('isAvailable', {
  //fioName: generateTestingFioAddress(),
  //  })

  //  expect(result.is_registered).to.equal(0)
  //})

  //it(`isAvailable false`, async () => {
  //const result = await fioSdk.genericAction('isAvailable', {
  //fioName: testFioAddressName
  //  })

  //  expect(result.is_registered).to.equal(1)
  //})

  //it(`getFioBalance for custom fioPublicKey`, async () => {
  //  const result = await fioSdk.genericAction('getFioBalance', {
  //  fioPublicKey: publicKey2
  //  })

  //  expect(result).to.have.all.keys('balance', 'available')
  //  expect(result.balance).to.be.a('number')
  //  expect(result.available).to.be.a('number')
  //})


  //it(`getFioAddresses`, async () => {
  //const result = await fioSdk.genericAction('getFioAddresses', { fioPublicKey: publicKey })

  //  expect(result).to.have.all.keys('fio_addresses', 'more')
  //  expect(result.fio_addresses).to.be.a('array')
  //})

  //it(`getFee`, async () => {
  //const result = await fioSdk.genericAction('getFee', {
  //endPoint: 'register_fio_address',
  //    fioAddress: ''
  //  })

  //  expect(result).to.have.all.keys('fee')
  //  expect(result.fee).to.be.a('number')
  //})

  //it(`getMultiplier`, async () => {
  //const result = await fioSdk.genericAction('getMultiplier', { })

  //  expect(result).to.be.a('number')
  //})

  //it(`getFee for BurnFioAddress`, async () => {
  //const result = await fioSdk.genericAction('getFeeForBurnFioAddress', {
  //fioAddress: newFioAddress
  //  })

  //  expect(result).to.have.all.keys('fee')
  //  expect(result.fee).to.be.a('number')
  //})

  //it(`Burn fio address`, async () => {
  //const result = await fioSdk.genericAction('burnFioAddress', {
  //fioAddress: newFioAddress,
  //    maxFee: defaultFee
  //  })

  //  expect(result).to.have.all.keys('status', 'fee_collected')
  //  expect(result.status).to.be.a('string')
  //  expect(result.fee_collected).to.be.a('number')
  //})
    }
}
