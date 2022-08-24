using FioSharp.Core;
using FioSharp.Core.Api.v1;
using FioSharp.Core.Helpers;
using FioSharp.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FioSharp.UnitTests
{
    public class ApiUnitTestCases
    {
        private FioBase fio;
        private FioConfigurator EosConfig;
        private FioApi DefaultApi;

        public ApiUnitTestCases(FioConfigurator eosConfig, FioApi api)
        {
            EosConfig = eosConfig;
            DefaultApi = api;
        }

        public ApiUnitTestCases(FioBase fio, FioConfigurator eosConfig, FioApi api)
        {
            this.fio = fio;
            EosConfig = eosConfig;
            DefaultApi = api;
        }

        //public async Task GetFioBalance()
        //{
        //    List<string> keys = (await EosConfig.SignProvider.GetAvailableKeys()).ToList();
        //    return DefaultApi.GetFioBalance(keys[0]);
        //}

        //public Task GetInfo()
        //{
        //    return DefaultApi.GetInfo();
        //}

        //public Task GetAccount()
        //{
        //    return DefaultApi.GetAccount("fio");
        //}

        //public Task<GetAbiResponse> GetReqObtAbi()
        //{
        //    return DefaultApi.GetAbi(new GetAbiRequest()
        //    {
        //        account_name = "fio.reqobt"
        //    });
        //}

        //public async Task GetRawCodeAndAbi()
        //{
        //    var result = await DefaultApi.GetRawCodeAndAbi(new GetRawCodeAndAbiRequest()
        //    {
        //        account_name = "fio.token"
        //    });

        //    var abiSerializer = new AbiSerializationProvider(DefaultApi);
        //    var abiObject = abiSerializer.DeserializePackedAbi(result.abi);
        //}

        //public async Task GetRawAbi()
        //{
        //    var result = await DefaultApi.GetRawAbi(new GetRawAbiRequest()
        //    {
        //        account_name = "fio.token"
        //    });

        //    var abiSerializer = new AbiSerializationProvider(DefaultApi);
        //    var abiObject = abiSerializer.DeserializePackedAbi(result.abi);
        //}

        //public Task AbiJsonToBin()
        //{
        //    return DefaultApi.AbiJsonToBin(new AbiJsonToBinRequest() {
        //        code = "fio.token",
        //        action = "transfer",
        //        args = new Dictionary<string, object>() {
        //            { "from", "eosio" },
        //            { "to", "eosio.names" },
        //            { "quantity", "1.0000 EOS" },
        //            { "memo", "hello crypto world!" }
        //        }
        //    });
        //}

        //public async Task AbiBinToJson()
        //{
        //    var binArgsResult = await DefaultApi.AbiJsonToBin(new AbiJsonToBinRequest()
        //    {
        //        code = "fio.token",
        //        action = "transfer",
        //        args = new Dictionary<string, object>() {
        //            { "from", "eosio" },
        //            { "to", "eosio.names" },
        //            { "quantity", "1.0000 EOS" },
        //            { "memo", "hello crypto world!" }
        //        }
        //    });

        //    await DefaultApi.AbiBinToJson(new AbiBinToJsonRequest()
        //    {
        //        code = "fio.token",
        //        action = "transfer",
        //        binargs = binArgsResult.binargs
        //    });
        //}

        //public async Task GetRequiredKeys()
        //{
        //    var getInfoResult = await DefaultApi.GetInfo();
        //    var getBlockResult = await DefaultApi.GetBlock(new GetBlockRequest()
        //    {
        //        block_num_or_id = getInfoResult.last_irreversible_block_num.ToString()
        //    });

        //    var trx = new Transaction()
        //    {
        //        //trx headers
        //        expiration = getInfoResult.head_block_time.AddSeconds(60), //expire Seconds
        //        ref_block_num = (UInt16)(getInfoResult.last_irreversible_block_num & 0xFFFF),
        //        ref_block_prefix = getBlockResult.ref_block_prefix,
        //        // trx info
        //        max_net_usage_words = 0,
        //        max_cpu_usage_ms = 0,
        //        delay_sec = 0,
        //        context_free_actions = new List<Core.Api.v1.Action>(),
        //        actions = new List<Core.Api.v1.Action>()
        //        {
        //            new Core.Api.v1.Action()
        //            {
        //                account = "fio.token",
        //                authorization = new List<PermissionLevel>()
        //                {
        //                    new PermissionLevel() {actor = "bepto5rwh5gy", permission = "active" }
        //                },
        //                name = "transfer",
        //                data = new Dictionary<string, object>() {
        //                    { "from", "bepto5rwh5gy" },
        //                    { "to", "bepto5rwh5gy" },
        //                    { "quantity", "1.0000 FIO" },
        //                    { "memo", "hello crypto world!" }
        //                }
        //            }
        //        },
        //        transaction_extensions = new List<Extension>()
        //    };

        //    int actionIndex = 0;
        //    var abiSerializer = new AbiSerializationProvider(DefaultApi);
        //    var abiResponses = await abiSerializer.GetTransactionAbis(trx);

        //    foreach (var action in trx.context_free_actions)
        //    {
        //        action.data = SerializationHelper.ByteArrayToHexString(abiSerializer.SerializeActionData(action, abiResponses[actionIndex++]));
        //    }

        //    foreach (var action in trx.actions)
        //    {
        //        action.data = SerializationHelper.ByteArrayToHexString(abiSerializer.SerializeActionData(action, abiResponses[actionIndex++]));
        //    }

        //    var getRequiredResult = await DefaultApi.GetRequiredKeys(new GetRequiredKeysRequest()
        //    {
        //        available_keys = new List<string>() { "FIO5g9NUxPpbiupHicH4ZMjqaLYLAghbARXMvJMkZv5uPx4MAbnZg" },
        //        transaction = trx
        //    });
        //}

        //public async Task GetBlock()
        //{
        //    var getInfoResult = await DefaultApi.GetInfo();
        //    var getBlockResult = await DefaultApi.GetBlock(new GetBlockRequest()
        //    {
        //        block_num_or_id = getInfoResult.last_irreversible_block_num.ToString()
        //    });
        //}

        //public async Task GetBlockHeaderState()
        //{
        //    var getInfoResult = await DefaultApi.GetInfo();
        //    var result = await DefaultApi.GetBlockHeaderState(new GetBlockHeaderStateRequest()
        //    {
        //        block_num_or_id = getInfoResult.head_block_num.ToString()
        //    });
        //}

        //public Task GetTableRows()
        //{
        //    return DefaultApi.GetTableRows(new GetTableRowsRequest()
        //    {
        //        json = true,
        //        code = "fio.token",
        //        scope = "FIO",
        //        table = "stat"
        //    });
        //}

        //public Task GetTableByScope()
        //{
        //    return DefaultApi.GetTableByScope(new GetTableByScopeRequest()
        //    {
        //        code = "fio.token",
        //        table = "accounts"
        //    });
        //}

        //public Task GetCurrencyBalance()
        //{
        //    return DefaultApi.GetCurrencyBalance(new GetCurrencyBalanceRequest()
        //    {
        //            code = "fio.token",
        //            account = "tester112345",
        //            symbol = "FIO"
        //    });
        //}

        //public Task GetCurrencyStats()
        //{
        //    return DefaultApi.GetCurrencyStats(new GetCurrencyStatsRequest()
        //    {
        //        code = "fio.token",
        //        symbol = "FIO"
        //    });
        //}

        //public Task GetProducers()
        //{
        //    return DefaultApi.GetProducers(new GetProducersRequest()
        //    {
        //        json = false,                    
        //    });
        //}

        //public Task GetProducerSchedule()
        //{
        //    return DefaultApi.GetProducerSchedule();
        //}

        //public Task GetScheduledTransactions()
        //{
        //    return DefaultApi.GetScheduledTransactions(new GetScheduledTransactionsRequest() {
        //        json = true
        //    });
        //}

        //public Task<PushTransactionResponse> PushTransaction()
        //{
        //    return CreateTransaction();
        //}

        //public Task GetActions()
        //{
        //    return DefaultApi.GetActions(new GetActionsRequest() {
        //        account_name = "fio"
        //    });
        //}

        //public async Task GetTransaction()
        //{
        //    var trxResult = await CreateTransaction();

        //    var result = await DefaultApi.GetTransaction(new GetTransactionRequest()
        //    {
        //        id = trxResult.transaction_id
        //    });
        //}

        //public Task GetKeyAccounts()
        //{
        //    return DefaultApi.GetKeyAccounts(new GetKeyAccountsRequest()
        //    {
        //        public_key = "FIO5g9NUxPpbiupHicH4ZMjqaLYLAghbARXMvJMkZv5uPx4MAbnZg"
        //    });
        //}

        //public Task GetControlledAccounts()
        //{
        //    return DefaultApi.GetControlledAccounts(new GetControlledAccountsRequest()
        //    {
        //        controlling_account = "fio"
        //    });
        //}

        

    }
}
