using FioSharp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FioSharp.Core.Api.v1
{
    /// <summary>
    /// FioApi defines api methods to interface with a http handler
    /// </summary>
    public class FioApi
    {
        public FioConfigurator Config { get; set; }
        public IHttpHandler HttpHandler { get; set; }

        /// <summary>
        /// Eos Client api constructor.
        /// </summary>
        /// <param name="config">Configures client parameters</param>
        /// <param name="httpHandler">Http handler implementation</param>
        public FioApi(FioConfigurator config, IHttpHandler httpHandler)
        {
            Config = config;
            HttpHandler = httpHandler;
        }

        public async Task<TResponseData> GetJson<TResponseData>(string endpoint)
        {
            var url = string.Format("{0}/v1/chain/{1}", Config.HttpEndpoint, endpoint);
            Console.WriteLine(url);
            return await HttpHandler.GetJsonAsync<TResponseData>(url);
        }
        public async Task<TResponseData> PostJson<TResponseData>(string endpoint, Dictionary<string, dynamic> data, bool reload = false)
        {
            var url = string.Format("{0}/v1/chain/{1}", Config.HttpEndpoint, endpoint);
            Console.WriteLine(url);
            return await HttpHandler.PostJsonAsync<TResponseData>(url, data);
        }
        public async Task<TResponseData> PostJsonWithCache<TResponseData>(string endpoint, Dictionary<string, dynamic> data, bool reload = false)
        {
            var url = string.Format("{0}/v1/chain/{1}", Config.HttpEndpoint, endpoint);
            Console.WriteLine(url);
            return await HttpHandler.PostJsonWithCacheAsync<TResponseData>(url, data, reload);
        }

        public async Task<PushTransactionResponse> PushTransaction(string[] signatures,
            string packedTrx)
        {
            return await PostJson<PushTransactionResponse>("push_transaction",
              new Dictionary<string, dynamic>()
              {
                { "signatures", signatures },
                { "compression", 0 },
                { "packed_context_free_data", "" },
                { "packed_trx", packedTrx }
              });
        }

        public async Task<GetFioBalanceResponse> GetFioBalance(string fioPublicKey)
        {
            return await PostJson<GetFioBalanceResponse>("get_fio_balance",
              new Dictionary<string, dynamic>()
              {
                { "fio_public_key", fioPublicKey }
              });
        }
        public async Task<GetFioNamesResponse> GetFioNames(string fioPublicKey)
        {
            return await PostJson<GetFioNamesResponse>("get_fio_names",
              new Dictionary<string, dynamic>()
              {
                { "fio_public_key", fioPublicKey }
              });
        }
        public async Task<GetFioAddressesResponse> GetFioAddresses(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetFioAddressesResponse>("get_fio_addresses",
              new Dictionary<string, dynamic>()
              {
                { "fio_public_key", fioPublicKey }, { "limit", limit }, { "offset", offset }
              });
        }
        public async Task<GetFioDomainsResponse> GetFioDomains(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetFioDomainsResponse>("get_fio_domains",
              new Dictionary<string, dynamic>()
              {
                { "fio_public_key", fioPublicKey }, { "limit", limit }, { "offset", offset }
              });
        }
        public async Task<AvailCheckResponse> AvailCheck(string fioName)
        {
            return await PostJson<AvailCheckResponse>("avail_check",
              new Dictionary<string, dynamic>()
              {
                { "fio_name", fioName }
              });
        }
        public async Task<GetPubAddressResponse> GetPubAddress(string fioAddress, string chainCode, string tokenCode)
        {
            return await PostJson<GetPubAddressResponse>("get_pub_address",
              new Dictionary<string, dynamic>()
              {
                { "fio_address", fioAddress }, { "chain_code", chainCode }, { "token_code", tokenCode }
              });
        }
        public async Task<GetPubAddressesResponse> GetPubAddresses(string fioAddress, int limit, int offset)
        {
            return await PostJson<GetPubAddressesResponse>("get_pub_addresses",
              new Dictionary<string, dynamic>()
              {
                { "fio_address", fioAddress }, { "limit", limit }, { "offset", offset }
              });
        }
        public async Task<GetSentFioRequestsResponse> GetSentFioRequests(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetSentFioRequestsResponse>("get_sent_fio_requests",
              new Dictionary<string, dynamic>()
              {
                { "fio_public_key", fioPublicKey }, { "limit", limit }, { "offset", offset }
              });
        }
        public async Task<GetReceivedFioRequestsResponse> GetReceivedFioRequests(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetReceivedFioRequestsResponse>("get_received_fio_requests",
              new Dictionary<string, dynamic>()
              {
                { "fio_public_key", fioPublicKey }, { "limit", limit }, { "offset", offset }
              });
        }
        public async Task<GetPendingFioRequestsResponse> GetPendingFioRequests(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetPendingFioRequestsResponse>("get_pending_fio_requests",
              new Dictionary<string, dynamic>()
              {
                { "fio_public_key", fioPublicKey }, { "limit", limit }, { "offset", offset }
              });
        }
        public async Task<GetCancelledFioRequestsResponse> GetCancelledFioRequests(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetCancelledFioRequestsResponse>("get_cancelled_fio_requests",
              new Dictionary<string, dynamic>()
              {
                { "fio_public_key", fioPublicKey }, { "limit", limit }, { "offset", offset }
              });
        }
        public async Task<GetObtDataResponse> GetObtData(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetObtDataResponse>("get_obt_data",
              new Dictionary<string, dynamic>()
              {
                { "fio_public_key", fioPublicKey }, { "limit", limit }, { "offset", offset }
              });
        }
        public async Task<GetLocksResponse> GetLocks(string fioPublicKey)
        {
            return await PostJson<GetLocksResponse>("get_locks",
              new Dictionary<string, dynamic>()
              {
                { "fio_public_key", fioPublicKey }
              });
        }
        public async Task<GetFeeResponse> GetFee(string endPoint, string fioAddress)
        {
            return await PostJson<GetFeeResponse>("get_fee",
              new Dictionary<string, dynamic>()
              {
                { "end_point", endPoint }, { "fio_address", fioAddress }
              });
        }
        public async Task<GetNftsFioAddressResponse> GetNftsFioAddress(string fioAddress, int limit, int offset)
        {
            return await PostJson<GetNftsFioAddressResponse>("get_nfts_fio_address",
              new Dictionary<string, dynamic>()
              {
                { "fio_address", fioAddress }, { "limit", limit }, { "offset", offset }
              });
        }
        public async Task<GetNftsContractResponse> GetNftsContract(string chainCode, string contractAddress, string tokenId, int limit, int offset)
        {
            return await PostJson<GetNftsContractResponse>("get_nfts_contract",
              new Dictionary<string, dynamic>()
              {
                { "chain_code", chainCode }, { "contract_address", contractAddress }, { "token_id", tokenId }, { "limit", limit }, { "offset", offset }
              });
        }
        public async Task<GetNftsHashResponse> GetNftsHash(string hash, int limit, int offset)
        {
            return await PostJson<GetNftsHashResponse>("get_nfts_hash",
              new Dictionary<string, dynamic>()
              {
                { "hash", hash }, { "limit", limit }, { "offset", offset }
              });
        }
        public async Task<GetActorResponse> GetActor(string fioPublicKey)
        {
            return await PostJson<GetActorResponse>("get_actor",
              new Dictionary<string, dynamic>()
              {
                { "fio_public_key", fioPublicKey }
              });
        }
        public async Task<GetAccountResponse> GetAccount(string accountName)
        {
            return await PostJson<GetAccountResponse>("get_account",
              new Dictionary<string, dynamic>()
              {
                { "account_name", accountName }
              });
        }
        public async Task<GetInfoResponse> GetInfo()
        {
            return await GetJson<GetInfoResponse>("get_info");
        }
        public async Task<GetBlockResponse> GetBlock(string blockNumOrId)
        {
            return await PostJson<GetBlockResponse>("get_block",
              new Dictionary<string, dynamic>()
              {
                { "block_num_or_id", blockNumOrId }
              });
        }
        public async Task<GetAbiResponse> GetAbi(string accountName, bool reload = true)
        {
            return await PostJsonWithCache<GetAbiResponse>("get_abi",
              new Dictionary<string, dynamic>()
              {
                { "account_name", accountName }
              }, reload: reload);
        }
        public async Task<GetRawAbiResponse> GetRawAbi(string accountName, bool reload = true)
        {
            return await PostJsonWithCache<GetRawAbiResponse>("get_raw_abi",
              new Dictionary<string, dynamic>()
              {
                { "account_name", accountName }
              }, reload: reload);
        }
        public async Task<GetProducersResponse> GetProducers(string limit, string lowerBound, bool json)
        {
            return await PostJson<GetProducersResponse>("get_producers",
              new Dictionary<string, dynamic>()
              {
                { "limit", limit }, { "lower_bound", lowerBound }, { "json", json }
              });
        }
        public async Task<GetTableRowsResponse> GetTableRows(string code, string scope, string table, string lowerBound, string upperBound, string keyType, string indexPosition, int limit, bool json, bool reverse)
        {
            return await PostJson<GetTableRowsResponse>("get_table_rows",
              new Dictionary<string, dynamic>()
              {
                { "code", code }, { "scope", scope }, { "table", table }, { "lower_bound", lowerBound }, { "upper_bound", upperBound }, { "key_type", keyType }, { "index_position", indexPosition }, { "limit", limit }, { "json", json }, { "reverse", reverse }
              });
        }
    }
}
