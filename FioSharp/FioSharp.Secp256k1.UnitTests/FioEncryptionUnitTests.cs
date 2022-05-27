using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

namespace FioSharp.Secp256k1.UnitTests
{
    public class FioEncryptionUnitTests
    {
        byte[] secret = HexStringToByteArray("02332627b9325cb70510a70f0f6be4bcb008fbbc7893ca51dedf5bf46aa740c0fc9d3fbd737d09a3c4046d221f4f1a323f515332c3fef46e7f075db561b1a2c9");
        byte[] plaintext = Encoding.ASCII.GetBytes("secret message");
        byte[] IV = HexStringToByteArray("f300888ca4f512cebdc0020ff0f7224c");
        byte[] cipherBuffer = HexStringToByteArray("f300888ca4f512cebdc0020ff0f7224c7f896315e90e172bed65d005138f224da7301d5563614e3955750e4480aabf7753f44b4975308aeb8e23c31e114962ab");

        //[SetUp]
        //public void Setup()
        //{

        //}

        [Test]
        public void TestEncryptValidates()
        {
            Exception e = Assert.Throws<Exception>(() =>
            {
                FioEncryptionManager.GetInstance().EncryptBytes(secret, plaintext, new byte[] { 1 });
            });

            Assert.NotNull(e);
            Assert.AreEqual("IV must be 16 bytes", e.Message);
        }

        [Test]
        public void TestDecryptValidates()
        {
            Exception e = Assert.Throws<Exception>(() =>
            {
                FioEncryptionManager.GetInstance().DecryptBytes(secret, cipherBuffer.Concat(new byte[] { 1 }).ToArray());
            });

            Assert.NotNull(e);
            Assert.AreEqual("Decryption failed", e.Message);
        }

        [Test]
        public void TestEncrypt()
        {
            byte[] c = FioEncryptionManager.GetInstance().EncryptBytes(secret, plaintext, IV);
            Assert.AreEqual(cipherBuffer, c);
        }

        [Test]
        public void TestDecrypt()
        {
            byte[] c = FioEncryptionManager.GetInstance().DecryptBytes(secret, cipherBuffer);
            Assert.AreEqual(plaintext, c);
        }

        [Test]
        public void TestRandomIV()
        {
            byte[] message = new byte[32];
            RandomNumberGenerator.Create().GetBytes(message);
            byte[] c = FioEncryptionManager.GetInstance().EncryptBytes(secret, message, IV);
            byte[] p = FioEncryptionManager.GetInstance().DecryptBytes(secret, c);
            Assert.AreEqual(message, p);
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            try
            {
                return Enumerable.Range(0, hex.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                     .ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new byte[] { 1 };
            }
            
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
