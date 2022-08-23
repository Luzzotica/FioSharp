using FioSharp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
  
namespace FioSharp.Core.Api.v1
{
    public interface ITransaction : ITransactionContent
    {
        string GetContract();
        string GetName();
    }

    public interface ITransactionContent
    {
        Dictionary<string, object> ToJsonObject();
    }

    public class newfundsreq_content : ITransactionContent
    {
        string payeePublicAddress;
        string amount;
        string chainCode;
        string tokenCode;
        string memo;
        string hash;
        string offlineUrl;

        public newfundsreq_content(string payeePublicAddress, string amount, string chainCode, string tokenCode, string memo = null, string hash = null, string offlineUrl = null)
        {
            this.payeePublicAddress = payeePublicAddress;
            this.amount = amount;
            this.chainCode = chainCode;
            this.tokenCode = tokenCode;
            this.memo = memo;
            this.hash = hash;
            this.offlineUrl = offlineUrl;
        }

        public Dictionary<string, object> ToJsonObject()
        {
            return new Dictionary<string, object>() {
                { "payee_public_address", payeePublicAddress }, { "amount", amount }, { "chain_code", chainCode }, { "token_code", tokenCode }, { "memo", memo }, { "hash", hash }, { "offline_url", offlineUrl }
            };
        }
    }

    public class recordobt_content : ITransactionContent
    {
        string payerPublicAddress;
        string payeePublicAddress;
        string amount;
        string chainCode;
        string tokenCode;
        string status;
        string obtId;
        string memo;
        string hash;
        string offlineUrl;

        public recordobt_content(string payerPublicAddress, string payeePublicAddress, string amount, string chainCode, string tokenCode, string status, string obtId, string memo = null, string hash = null, string offlineUrl = null)
        {
            this.payerPublicAddress = payerPublicAddress;
            this.payeePublicAddress = payeePublicAddress;
            this.amount = amount;
            this.chainCode = chainCode;
            this.tokenCode = tokenCode;
            this.status = status;
            this.obtId = obtId;
            this.memo = memo;
            this.hash = hash;
            this.offlineUrl = offlineUrl;
        }

        public Dictionary<string, object> ToJsonObject()
        {
            return new Dictionary<string, object>() {
                { "payer_public_address", payerPublicAddress }, { "payee_public_address", payeePublicAddress }, { "amount", amount }, { "chain_code", chainCode }, { "token_code", tokenCode }, { "status", status }, { "obt_id", obtId}, { "memo", memo }, { "hash", hash }, { "offline_url", offlineUrl }
            };
        }
    }
  
    public class trnsfiopubky : ITransaction
    {
        string contract = "fio.token";
        string name = "trnsfiopubky";
        public string payeePublicKey;
        public ulong amount;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public trnsfiopubky(string payeePublicKey, ulong amount, ulong maxFee, string tpid, string actor)
        {
            this.payeePublicKey = payeePublicKey;
            this.amount = amount;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "payee_public_key", payeePublicKey }, { "amount", amount }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class trnsloctoks : ITransaction
    {
        string contract = "fio.token";
        string name = "trnsloctoks";
        public string payeePublicKey;
        public ulong canVote;
        public List<object> periods;
        public ulong amount;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public trnsloctoks(string payeePublicKey, ulong canVote, List<object> periods, ulong amount, ulong maxFee, string tpid, string actor)
        {
            this.payeePublicKey = payeePublicKey;
            this.canVote = canVote;
            this.periods = periods;
            this.amount = amount;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "payee_public_key", payeePublicKey }, { "can_vote", canVote }, { "periods", periods }, { "amount", amount }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class addaddress : ITransaction
    {
        string contract = "fio.address";
        string name = "addaddress";
        public string fioAddress;
        public List<object> publicAddresses;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public addaddress(string fioAddress, List<object> publicAddresses, ulong maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.publicAddresses = publicAddresses;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "public_addresses", publicAddresses }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class remaddress : ITransaction
    {
        string contract = "fio.address";
        string name = "remaddress";
        public string fioAddress;
        public List<object> publicAddresses;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public remaddress(string fioAddress, List<object> publicAddresses, ulong maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.publicAddresses = publicAddresses;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "public_addresses", publicAddresses }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class remalladdr : ITransaction
    {
        string contract = "fio.address";
        string name = "remalladdr";
        public string fioAddress;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public remalladdr(string fioAddress, ulong maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class newfundsreq : ITransaction
    {
        string contract = "fio.reqobt";
        string name = "newfundsreq";
        public string payerFioAddress;
        public string payeeFioAddress;
        public string tpid;
        public object content;
        public ulong maxFee;
        public string actor;

        public newfundsreq(string payerFioAddress, string payeeFioAddress, string tpid, object content, ulong maxFee, string actor)
        {
            this.payerFioAddress = payerFioAddress;
            this.payeeFioAddress = payeeFioAddress;
            this.tpid = tpid;
            this.content = content;
            this.maxFee = maxFee;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "payer_fio_address", payerFioAddress }, { "payee_fio_address", payeeFioAddress }, { "tpid", tpid }, { "content", content }, { "max_fee", maxFee }, { "actor", actor }
            };
        }
    }
          
    public class cancelfndreq : ITransaction
    {
        string contract = "fio.reqobt";
        string name = "cancelfndreq";
        public string fioRequestId;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public cancelfndreq(string fioRequestId, ulong maxFee, string tpid, string actor)
        {
            this.fioRequestId = fioRequestId;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_request_id", fioRequestId }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class rejectfndreq : ITransaction
    {
        string contract = "fio.reqobt";
        string name = "rejectfndreq";
        public string fioRequestId;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public rejectfndreq(string fioRequestId, ulong maxFee, string tpid, string actor)
        {
            this.fioRequestId = fioRequestId;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_request_id", fioRequestId }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class recordobt : ITransaction
    {
        string contract = "fio.reqobt";
        string name = "recordobt";
        public string payerFioAddress;
        public string payeeFioAddress;
        public object content;
        public string fioRequestId;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public recordobt(string payerFioAddress, string payeeFioAddress, object content, string fioRequestId, ulong maxFee, string tpid, string actor)
        {
            this.payerFioAddress = payerFioAddress;
            this.payeeFioAddress = payeeFioAddress;
            this.content = content;
            this.fioRequestId = fioRequestId;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "payer_fio_address", payerFioAddress }, { "payee_fio_address", payeeFioAddress }, { "content", content }, { "fio_request_id", fioRequestId }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class regaddress : ITransaction
    {
        string contract = "fio.address";
        string name = "regaddress";
        public string fioAddress;
        public string ownerFioPublicKey;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public regaddress(string fioAddress, string ownerFioPublicKey, ulong maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.ownerFioPublicKey = ownerFioPublicKey;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "owner_fio_public_key", ownerFioPublicKey }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class renewaddress : ITransaction
    {
        string contract = "fio.address";
        string name = "renewaddress";
        public string fioAddress;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public renewaddress(string fioAddress, ulong maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class addbundles : ITransaction
    {
        string contract = "fio.address";
        string name = "addbundles";
        public string fioAddress;
        public ulong bundleSets;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public addbundles(string fioAddress, ulong bundleSets, ulong maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.bundleSets = bundleSets;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "bundle_sets", bundleSets }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class xferaddress : ITransaction
    {
        string contract = "fio.address";
        string name = "xferaddress";
        public string fioAddress;
        public string newOwnerFioPublicKey;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public xferaddress(string fioAddress, string newOwnerFioPublicKey, ulong maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.newOwnerFioPublicKey = newOwnerFioPublicKey;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "new_owner_fio_public_key", newOwnerFioPublicKey }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class burnaddress : ITransaction
    {
        string contract = "fio.address";
        string name = "burnaddress";
        public string fioAddress;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public burnaddress(string fioAddress, ulong maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class regdomain : ITransaction
    {
        string contract = "fio.address";
        string name = "regdomain";
        public string fioDomain;
        public string ownerFioPublicKey;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public regdomain(string fioDomain, string ownerFioPublicKey, ulong maxFee, string tpid, string actor)
        {
            this.fioDomain = fioDomain;
            this.ownerFioPublicKey = ownerFioPublicKey;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_domain", fioDomain }, { "owner_fio_public_key", ownerFioPublicKey }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class renewdomain : ITransaction
    {
        string contract = "fio.address";
        string name = "renewdomain";
        public string fioDomain;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public renewdomain(string fioDomain, ulong maxFee, string tpid, string actor)
        {
            this.fioDomain = fioDomain;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_domain", fioDomain }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class setdomainpub : ITransaction
    {
        string contract = "fio.address";
        string name = "setdomainpub";
        public string fioDomain;
        public ulong isPublic;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public setdomainpub(string fioDomain, ulong isPublic, ulong maxFee, string tpid, string actor)
        {
            this.fioDomain = fioDomain;
            this.isPublic = isPublic;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_domain", fioDomain }, { "is_public", isPublic }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class xferdomain : ITransaction
    {
        string contract = "fio.address";
        string name = "xferdomain";
        public string fioDomain;
        public string newOwnerFioPublicKey;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public xferdomain(string fioDomain, string newOwnerFioPublicKey, ulong maxFee, string tpid, string actor)
        {
            this.fioDomain = fioDomain;
            this.newOwnerFioPublicKey = newOwnerFioPublicKey;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_domain", fioDomain }, { "new_owner_fio_public_key", newOwnerFioPublicKey }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class addnft : ITransaction
    {
        string contract = "fio.address";
        string name = "addnft";
        public string fioAddress;
        public List<object> nfts;
        public ulong maxFee;
        public string actor;
        public string tpid;

        public addnft(string fioAddress, List<object> nfts, ulong maxFee, string actor, string tpid)
        {
            this.fioAddress = fioAddress;
            this.nfts = nfts;
            this.maxFee = maxFee;
            this.actor = actor;
            this.tpid = tpid;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "nfts", nfts }, { "max_fee", maxFee }, { "actor", actor }, { "tpid", tpid }
            };
        }
    }
          
    public class remnft : ITransaction
    {
        string contract = "fio.address";
        string name = "remnft";
        public string fioAddress;
        public List<object> nfts;
        public ulong maxFee;
        public string actor;
        public string tpid;

        public remnft(string fioAddress, List<object> nfts, ulong maxFee, string actor, string tpid)
        {
            this.fioAddress = fioAddress;
            this.nfts = nfts;
            this.maxFee = maxFee;
            this.actor = actor;
            this.tpid = tpid;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "nfts", nfts }, { "max_fee", maxFee }, { "actor", actor }, { "tpid", tpid }
            };
        }
    }
          
    public class remallnfts : ITransaction
    {
        string contract = "fio.address";
        string name = "remallnfts";
        public string fioAddress;
        public ulong maxFee;
        public string actor;
        public string tpid;

        public remallnfts(string fioAddress, ulong maxFee, string actor, string tpid)
        {
            this.fioAddress = fioAddress;
            this.maxFee = maxFee;
            this.actor = actor;
            this.tpid = tpid;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "max_fee", maxFee }, { "actor", actor }, { "tpid", tpid }
            };
        }
    }
          
    public class stakefio : ITransaction
    {
        string contract = "fio.staking";
        string name = "stakefio";
        public ulong amount;
        public string fioAddress;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public stakefio(ulong amount, string fioAddress, ulong maxFee, string tpid, string actor)
        {
            this.amount = amount;
            this.fioAddress = fioAddress;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "amount", amount }, { "fio_address", fioAddress }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class unstakefio : ITransaction
    {
        string contract = "fio.staking";
        string name = "unstakefio";
        public ulong amount;
        public string fioAddress;
        public ulong maxFee;
        public string tpid;
        public string actor;

        public unstakefio(ulong amount, string fioAddress, ulong maxFee, string tpid, string actor)
        {
            this.amount = amount;
            this.fioAddress = fioAddress;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "amount", amount }, { "fio_address", fioAddress }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class voteproducer : ITransaction
    {
        string contract = "eosio";
        string name = "voteproducer";
        public List<object> producers;
        public string fioAddress;
        public string actor;
        public ulong maxFee;

        public voteproducer(List<object> producers, string fioAddress, string actor, ulong maxFee)
        {
            this.producers = producers;
            this.fioAddress = fioAddress;
            this.actor = actor;
            this.maxFee = maxFee;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "producers", producers }, { "fio_address", fioAddress }, { "actor", actor }, { "max_fee", maxFee }
            };
        }
    }
          
    public class voteproxy : ITransaction
    {
        string contract = "eosio";
        string name = "voteproxy";
        public string proxy;
        public string fioAddress;
        public string actor;
        public ulong maxFee;

        public voteproxy(string proxy, string fioAddress, string actor, ulong maxFee)
        {
            this.proxy = proxy;
            this.fioAddress = fioAddress;
            this.actor = actor;
            this.maxFee = maxFee;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "proxy", proxy }, { "fio_address", fioAddress }, { "actor", actor }, { "max_fee", maxFee }
            };
        }
    }
          
    public class regproxy : ITransaction
    {
        string contract = "eosio";
        string name = "regproxy";
        public string fioAddress;
        public string actor;
        public ulong maxFee;

        public regproxy(string fioAddress, string actor, ulong maxFee)
        {
            this.fioAddress = fioAddress;
            this.actor = actor;
            this.maxFee = maxFee;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "actor", actor }, { "max_fee", maxFee }
            };
        }
    }
          
    public class unregproxy : ITransaction
    {
        string contract = "eosio";
        string name = "unregproxy";
        public string fioAddress;
        public ulong actor;
        public ulong maxFee;

        public unregproxy(string fioAddress, ulong actor, ulong maxFee)
        {
            this.fioAddress = fioAddress;
            this.actor = actor;
            this.maxFee = maxFee;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "actor", actor }, { "max_fee", maxFee }
            };
        }
    }
          
    public class regproducer : ITransaction
    {
        string contract = "eosio";
        string name = "regproducer";
        public string fioAddress;
        public string fioPubKey;
        public string url;
        public ulong location;
        public string actor;
        public ulong maxFee;

        public regproducer(string fioAddress, string fioPubKey, string url, ulong location, string actor, ulong maxFee)
        {
            this.fioAddress = fioAddress;
            this.fioPubKey = fioPubKey;
            this.url = url;
            this.location = location;
            this.actor = actor;
            this.maxFee = maxFee;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "fio_pub_key", fioPubKey }, { "url", url }, { "location", location }, { "actor", actor }, { "max_fee", maxFee }
            };
        }
    }
          
    public class unregprod : ITransaction
    {
        string contract = "eosio";
        string name = "unregprod";
        public string fioAddress;
        public string actor;
        public ulong maxFee;

        public unregprod(string fioAddress, string actor, ulong maxFee)
        {
            this.fioAddress = fioAddress;
            this.actor = actor;
            this.maxFee = maxFee;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "actor", actor }, { "max_fee", maxFee }
            };
        }
    }
          
    public class setfeevote : ITransaction
    {
        string contract = "fio.fee";
        string name = "setfeevote";
        public List<object> feeRatios;
        public string actor;

        public setfeevote(List<object> feeRatios, string actor)
        {
            this.feeRatios = feeRatios;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fee_ratios", feeRatios }, { "actor", actor }
            };
        }
    }
          
    public class setfeemult : ITransaction
    {
        string contract = "fio.fee";
        string name = "setfeemult";
        public ulong multiplier;
        public string actor;

        public setfeemult(ulong multiplier, string actor)
        {
            this.multiplier = multiplier;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "multiplier", multiplier }, { "actor", actor }
            };
        }
    }
          
    public class computefees : ITransaction
    {
        string contract = "fio.fee";
        string name = "computefees";
        

        public computefees()
        {
            
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                
            };
        }
    }
          
    public class bundlevote : ITransaction
    {
        string contract = "fio.fee";
        string name = "bundlevote";
        public ulong bundledTransactions;
        public string actor;

        public bundlevote(ulong bundledTransactions, string actor)
        {
            this.bundledTransactions = bundledTransactions;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "bundled_transactions", bundledTransactions }, { "actor", actor }
            };
        }
    }
          
    public class tpidclaim : ITransaction
    {
        string contract = "fio.treasury";
        string name = "tpidclaim";
        public string actor;

        public tpidclaim(string actor)
        {
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "actor", actor }
            };
        }
    }
          
    public class bpclaim : ITransaction
    {
        string contract = "fio.treasury";
        string name = "bpclaim";
        public string fioAddress;
        public string actor;

        public bpclaim(string fioAddress, string actor)
        {
            this.fioAddress = fioAddress;
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "actor", actor }
            };
        }
    }
          
    public class burnexpired : ITransaction
    {
        string contract = "fio.address";
        string name = "burnexpired";
        

        public burnexpired()
        {
            
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                
            };
        }
    }
          
    public class burnnfts : ITransaction
    {
        string contract = "fio.address";
        string name = "burnnfts";
        public string actor;

        public burnnfts(string actor)
        {
            this.actor = actor;
        }

        public string GetContract() 
        {
            return contract;
        }

        public string GetName() 
        {
            return name;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "actor", actor }
            };
        }
    }
        
}
