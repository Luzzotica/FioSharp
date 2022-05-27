using System;
using System.Collections.Generic;
using NUnit.Framework;
using NBitcoin;
using NBitcoin.Crypto;
using System.Text;

namespace EosSharp.UnitTests
{
    public class EncryptionTests
    {
        
        Dictionary<string, object> newFundsContent = new Dictionary<string, object>() {
                { "payee_public_address", "purse.alice" },
                { "amount", "1" },
                { "chain_code", "FIO" },
                { "token_code", "FIO" },
                { "memo", null },
                { "hash", null },
                { "offline_url", null }
            };

        string newFundsContentHex = "0B70757273652E616C69636501310346494F0346494F000000";

        [Test]
        public void Encryption()
        {
            Key privateKeyAlice = new Key(Hashes.SHA256(Encoding.UTF8.GetBytes("alice")));
            byte[] publicKeyAlice = privateKeyAlice.PubKey.ToBytes();
            Key privateKeyBob = new Key(Hashes.SHA256(Encoding.UTF8.GetBytes("bob")));
            byte[] publicKeyBob = privateKeyBob.PubKey.ToBytes();

            byte[] IV = new byte[] { 0xf3, 0x00, 0x88, 0x8c, 0xa4, 0xf5, 0x12, 0xc, 0xe, 0xbd, 0xc0, 0x02, 0x0f, 0xf0, 0xf7, 0x22, 0x4c }; //Buffer.from('f300888ca4f512cebdc0020ff0f7224c', 'hex');
            string newFundsContentCipherBase64 = "8wCIjKT1Es69wAIP8PciTOB8F09qqDGdsq0XriIWcOkqpZe9q4FwKu3SGILtnAWtJGETbcAqd3zX7NDptPUQsS1ZfEPiK6Hv0nJyNbxwiQc=";

            Assert.IsTrue(false);
        }

        [Test]
        public void Decryption()
        {

        }
    }
}
