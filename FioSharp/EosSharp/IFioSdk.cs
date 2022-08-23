using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FioSharp.Core.Api.v1;
using FioSharp.Models;

namespace FioSharp
{
    interface IFioSdk
    {
        Task Init();

        //public MnemonicKey CreatePrivateKey(byte[] entropy = null);
        //public MnemonicKey CreatePrivateKeyMnemonic(string mnemonic);
        //public string DerivePublicKey(string fioPrivateKey);
        //public bool IsChainCodeValid(string chainCode);
        //public bool IsTokenCodeValid(string tokenCode);
        //public bool IsFioAddressValid(string fioAddress);
        //public bool IsFioDomainValid(string fioDomain);
        //public bool IsFioPublicKeyValid(string fioPublicKey);
        //public bool IsPublicAddressValid(string publicAddress);
        //public double amountToSUF(double amount);
        //public double SUFToAmount(double suf);

        Task<string> DHEncrypt(string publicKey, string fioContentType, Dictionary<string, dynamic> content);
        Task<Dictionary<string, dynamic>> DHDecrypt(string publicKey, string fioContentType, string content);
        Task<string> PushTransaction(List<Core.Api.v1.Action> actions,
            List<Core.Api.v1.Action> contextFreeActions = null,
            List<Extension> transactionExtensions = null);

        Fio GetFio();
        FioApi GetFioApi();
        string GetPublicKey();
        string GetTehcnologyProviderId();
        string GetActor();
    }
}
