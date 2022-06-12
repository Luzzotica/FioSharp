using FioSharp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FioSharp.Core.Api.v1
{
	/// <summary>
    /// EosApi defines api methods to interface with a http handler
    /// </summary>
    public class FioApi
    {
        public const string get_fio_balance = "get_fio_balance";
        public const string get_fio_names = "get_fio_names";
        public const string get_fio_addresses = "get_fio_addresses";
        public const string get_fio_domains = "get_fio_domains";
        public const string avail_check = "avail_check";
        public const string get_pub_address = "get_pub_address";
        public const string get_pub_addresses = "get_pub_addresses";
        public const string get_sent_fio_requests = "get_sent_fio_requests";
        public const string get_received_fio_requests = "get_received_fio_requests";
        public const string get_pending_fio_requests = "get_pending_fio_requests";
        public const string get_cancelled_fio_requests = "get_cancelled_fio_requests";
        public const string get_obt_data = "get_obt_data";
        public const string get_locks = "get_locks";
        public const string get_fee = "get_fee";
        public const string get_nfts_fio_address = "get_nfts_fio_address";
        public const string get_nfts_contract = "get_nfts_contract";
        public const string get_nfts_hash = "get_nfts_hash";
        public const string get_actor = "get_actor";
        public const string get_account = "get_account";
        public const string get_info = "get_info";
        public const string get_block = "get_block";
        public const string get_abi = "get_abi";
        public const string get_raw_abi = "get_raw_abi";
        public const string get_producers = "get_producers";
        public const string get_table_rows = "get_table_rows";
        public const string push_transaction = "push_transaction";

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

        public async Task<GetFioBalanceResponse> GetFioBalance(string fioPublicKey)
        {
            return await PostJson<GetFioBalanceResponse>(FioApi.get_fio_balance,
                new Dictionary<string, dynamic>()
                {
                    { "fio_public_key", fioPublicKey }
                });
        }

        public async Task<GetFioNamesResponse> GetFioNames(string fioPublicKey)
        {
            return await PostJson<GetFioNamesResponse>(FioApi.get_fio_names,
                new Dictionary<string, dynamic>()
                {
                    { "fio_public_key", fioPublicKey }
                });
        }

        public async Task<GetFioAddressesResponse> GetFioAddresses(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetFioAddressesResponse>(FioApi.get_fio_addresses,
                new Dictionary<string, dynamic>()
                {
                    { "fio_public_key", fioPublicKey },
                    { "limit", limit },
                    { "offset", offset }
                });
        }

        public async Task<GetFioDomainsResponse> GetFioDomains(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetFioDomainsResponse>(FioApi.get_fio_domains,
                new Dictionary<string, dynamic>()
                {
                    { "fio_public_key", fioPublicKey },
                    { "limit", limit },
                    { "offset", offset }
                });
        }

        public async Task<AvailCheckResponse> AvailCheck(string fioName)
        {
            return await PostJson<AvailCheckResponse>(FioApi.avail_check,
                new Dictionary<string, dynamic>()
                {
                    { "fio_name", fioName },
                });
        }

        public async Task<GetPubAddressRequest> GetPublicAddress(string fioAddress, string chainCode, string tokenCode)
        {
            return await PostJson<GetPubAddressRequest>(FioApi.get_pub_address,
                new Dictionary<string, dynamic>()
                {
                    { "fio_address", fioAddress },
                    { "chain_code", chainCode },
                    { "token_code", tokenCode }
                });
        }

        public async Task<GetPubAddressesResponse> GetPublicAddresses(string fioAddress, int limit, int offset)
        {
            return await PostJson<GetPubAddressesResponse>(FioApi.get_pub_addresses,
                new Dictionary<string, dynamic>()
                {
                    { "fio_address", fioAddress },
                    { "limit", limit },
                    { "offset", offset }
                });
        }

        public async Task<GetFioRequestsResponse> GetSentFioRequests(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetFioRequestsResponse>(FioApi.get_sent_fio_requests,
                new Dictionary<string, dynamic>()
                {
                    { "fio_public_key", fioPublicKey },
                    { "limit", limit },
                    { "offset", offset }
                });
        }

        public async Task<GetFioRequestsResponse> GetReceivedFioRequests(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetFioRequestsResponse>(FioApi.get_received_fio_requests,
                new Dictionary<string, dynamic>()
                {
                    { "fio_public_key", fioPublicKey },
                    { "limit", limit },
                    { "offset", offset }
                });
        }

        public async Task<GetFioRequestsResponse> GetPendingFioRequests(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetFioRequestsResponse>(FioApi.get_pending_fio_requests,
                new Dictionary<string, dynamic>()
                {
                    { "fio_public_key", fioPublicKey },
                    { "limit", limit },
                    { "offset", offset }
                });
        }

        public async Task<GetFioRequestsResponse> GetCancelledFioRequests(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetFioRequestsResponse>(FioApi.get_cancelled_fio_requests,
                new Dictionary<string, dynamic>()
                {
                    { "fio_public_key", fioPublicKey },
                    { "limit", limit },
                    { "offset", offset }
                });
        }

        public async Task<GetObtDataResponse> GetObtData(string fioPublicKey, int limit, int offset)
        {
            return await PostJson<GetObtDataResponse>(FioApi.get_obt_data,
                new Dictionary<string, dynamic>()
                {
                    { "fio_public_key", fioPublicKey },
                    { "limit", limit },
                    { "offset", offset }
                });
        }

        public async Task<GetLocksResponse> GetLocks(string fioPublicKey)
        {
            return await PostJson<GetLocksResponse>(FioApi.get_locks,
                new Dictionary<string, dynamic>()
                {
                    { "fio_public_key", fioPublicKey }
                });
        }

        public async Task<GetFeeResponse> GetFee(string endpoint, string fioAddress)
        {
            return await PostJson<GetFeeResponse>(FioApi.get_fee,
                new Dictionary<string, dynamic>()
                {
                    { "end_point", endpoint },
                    { "fio_address", fioAddress }
                });
        }

        public async Task<GetNftsResponse> GetNftsFioAddress(string fioAddress, int limit, int offset)
        {
            return await PostJson<GetNftsResponse>(FioApi.get_nfts_fio_address,
                new Dictionary<string, dynamic>()
                {
                    { "fio_address", fioAddress },
                    { "limit", limit },
                    { "offset", offset },
                });
        }

        public async Task<GetNftsResponse> GetNftsContract(string contractAddress, string chainCode, string tokenId, int limit, int offset)
        {
            return await PostJson<GetNftsResponse>(FioApi.get_nfts_contract,
                new Dictionary<string, dynamic>()
                {
                    { "contract_address", contractAddress },
                    { "chain_code", chainCode },
                    { "token_id", tokenId },
                    { "limit", limit },
                    { "offset", offset },
                });
        }

        public async Task<GetNftsResponse> GetNftsHash(string hash, int limit, int offset)
        {
            return await PostJson<GetNftsResponse>(FioApi.get_nfts_hash,
                new Dictionary<string, dynamic>()
                {
                    { "hash", hash },
                    { "limit", limit },
                    { "offset", offset },
                });
        }

        public async Task<GetActorResponse> GetActor(string fioPublicKey)
        {
            return await PostJson<GetActorResponse>(FioApi.get_actor,
                new Dictionary<string, dynamic>()
                {
                    { "fio_public_key", fioPublicKey }
                });
        }

        public async Task<GetAccountResponse> GetAccount(string accountName)
        {
            return await PostJson<GetAccountResponse>(FioApi.get_account,
                new Dictionary<string, dynamic>()
                {
                    { "account_name", accountName }
                });
        }

        /// <summary>
        /// Query for blockchain information
        /// </summary>
        /// <returns>Blockchain information</returns>
        public async Task<GetInfoResponse> GetInfo()
        {
            return await GetJson<GetInfoResponse>(FioApi.get_info);
        }

        /// <summary>
        /// Query for blockchain block information
        /// </summary>
        /// <param name="blockNumOrId">block number or id to query information</param>
        /// <returns>block information</returns>
        public async Task<GetBlockResponse> GetBlock(string blockNumOrId)
        {
            return await PostJson<GetBlockResponse>(FioApi.get_block,
                new Dictionary<string, dynamic>()
                {
                    { "block_num_or_id", blockNumOrId }
                });
        }

        /// <summary>
        /// Query for smart contract abi detailed information
        /// </summary>
        /// <param name="accountName">smart contract account name</param>
        /// <returns></returns>
        public async Task<Abi> GetAbi(string accountName, bool reload = false)
        {
            GetAbiResponse resp = await PostJsonWithCache<GetAbiResponse>(FioApi.get_abi,
                new Dictionary<string, dynamic>()
                {
                    { "account_name", accountName }
                }, reload: reload);
            return resp.abi;
        }

        /// <summary>
        /// Query for smart contract abi detailed information
        /// </summary>
        /// <param name="accountName">smart contract account name</param>
        /// <returns>smart contract abi information as Base64FcString</returns>
        public async Task<GetRawAbiResponse> GetRawAbi(string accountName, bool reload = false)
        {
            return await PostJsonWithCache<GetRawAbiResponse>(FioApi.get_raw_abi,
                new Dictionary<string, dynamic>()
                {
                    { "account_name", accountName }
                }, reload: reload);
        }

        public async Task<GetProducersResponse> GetProducers(string limit, string lower_bound)
        {
            return await PostJson<GetProducersResponse>(FioApi.get_producers,
                new Dictionary<string, dynamic>()
                {
                    { "limit", limit },
                    { "lower_bound", lower_bound },
                    { "json", true }
                });
        }

        public async Task<PushTransactionResponse> PushTransaction(string[] signatures,
            string packedTrx)
        {
            return await PostJson<PushTransactionResponse>(FioApi.push_transaction,
                new Dictionary<string, dynamic>()
                {
                    { "signatures", signatures },
                    { "compression", 0 },
                    { "packed_context_free_data", "" },
                    { "packed_trx", packedTrx }
                });
        }
    }
}
