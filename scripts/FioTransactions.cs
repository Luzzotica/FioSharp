using FioSharp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
  
namespace FioSharp.Core.Api.v1
{
    public interface ITransactionData
    {
      Dictionary<string, object> ToJsonObject();
    }
  
    public class trnsfiopubky : ITransactionData 
    {
        public string payeePublicKey;
        public int amount;
        public int maxFee;
        public string tpid;
        public string actor;

        public trnsfiopubky(string payeePublicKey, int amount, int maxFee, string tpid, string actor)
        {
            this.payeePublicKey = payeePublicKey;
            this.amount = amount;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "payee_public_key", payeePublicKey }, { "amount", amount }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class trnsloctoks : ITransactionData 
    {
        public string payeePublicKey;
        public int canVote;
        public List<object> periods;
        public int amount;
        public int maxFee;
        public string tpid;
        public string actor;

        public trnsloctoks(string payeePublicKey, int canVote, List<object> periods, int amount, int maxFee, string tpid, string actor)
        {
            this.payeePublicKey = payeePublicKey;
            this.canVote = canVote;
            this.periods = periods;
            this.amount = amount;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "payee_public_key", payeePublicKey }, { "can_vote", canVote }, { "periods", periods }, { "amount", amount }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class addaddress : ITransactionData 
    {
        public string fioAddress;
        public List<object> publicAddresses;
        public int maxFee;
        public string tpid;
        public string actor;

        public addaddress(string fioAddress, List<object> publicAddresses, int maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.publicAddresses = publicAddresses;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "public_addresses", publicAddresses }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class remaddress : ITransactionData 
    {
        public string fioAddress;
        public List<object> publicAddresses;
        public int maxFee;
        public string tpid;
        public string actor;

        public remaddress(string fioAddress, List<object> publicAddresses, int maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.publicAddresses = publicAddresses;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "public_addresses", publicAddresses }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class remalladdr : ITransactionData 
    {
        public string fioAddress;
        public int maxFee;
        public string tpid;
        public string actor;

        public remalladdr(string fioAddress, int maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class newfundsreq : ITransactionData 
    {
        public string payerFioAddress;
        public string payeeFioAddress;
        public string tpid;
        public object content;
        public int maxFee;
        public string actor;

        public newfundsreq(string payerFioAddress, string payeeFioAddress, string tpid, object content, int maxFee, string actor)
        {
            this.payerFioAddress = payerFioAddress;
            this.payeeFioAddress = payeeFioAddress;
            this.tpid = tpid;
            this.content = content;
            this.maxFee = maxFee;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "payer_fio_address", payerFioAddress }, { "payee_fio_address", payeeFioAddress }, { "tpid", tpid }, { "content", content }, { "max_fee", maxFee }, { "actor", actor }
            };
        }
    }
          
    public class cancelfndreq : ITransactionData 
    {
        public string fioRequestId;
        public int maxFee;
        public string tpid;
        public string actor;

        public cancelfndreq(string fioRequestId, int maxFee, string tpid, string actor)
        {
            this.fioRequestId = fioRequestId;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_request_id", fioRequestId }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class rejectfndreq : ITransactionData 
    {
        public string fioRequestId;
        public int maxFee;
        public string tpid;
        public string actor;

        public rejectfndreq(string fioRequestId, int maxFee, string tpid, string actor)
        {
            this.fioRequestId = fioRequestId;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_request_id", fioRequestId }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class recordobt : ITransactionData 
    {
        public string payerFioAddress;
        public string payeeFioAddress;
        public object content;
        public string fioRequestId;
        public int maxFee;
        public string tpid;
        public string actor;

        public recordobt(string payerFioAddress, string payeeFioAddress, object content, string fioRequestId, int maxFee, string tpid, string actor)
        {
            this.payerFioAddress = payerFioAddress;
            this.payeeFioAddress = payeeFioAddress;
            this.content = content;
            this.fioRequestId = fioRequestId;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "payer_fio_address", payerFioAddress }, { "payee_fio_address", payeeFioAddress }, { "content", content }, { "fio_request_id", fioRequestId }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class regaddress : ITransactionData 
    {
        public string fioAddress;
        public string ownerFioPublicKey;
        public int maxFee;
        public string tpid;
        public string actor;

        public regaddress(string fioAddress, string ownerFioPublicKey, int maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.ownerFioPublicKey = ownerFioPublicKey;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "owner_fio_public_key", ownerFioPublicKey }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class renewaddress : ITransactionData 
    {
        public string fioAddress;
        public int maxFee;
        public string tpid;
        public string actor;

        public renewaddress(string fioAddress, int maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class addbundles : ITransactionData 
    {
        public string fioAddress;
        public int bundleSets;
        public int maxFee;
        public string tpid;
        public string actor;

        public addbundles(string fioAddress, int bundleSets, int maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.bundleSets = bundleSets;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "bundle_sets", bundleSets }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class xferaddress : ITransactionData 
    {
        public string fioAddress;
        public string newOwnerFioPublicKey;
        public int maxFee;
        public string tpid;
        public string actor;

        public xferaddress(string fioAddress, string newOwnerFioPublicKey, int maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.newOwnerFioPublicKey = newOwnerFioPublicKey;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "new_owner_fio_public_key", newOwnerFioPublicKey }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class burnaddress : ITransactionData 
    {
        public string fioAddress;
        public int maxFee;
        public string tpid;
        public string actor;

        public burnaddress(string fioAddress, int maxFee, string tpid, string actor)
        {
            this.fioAddress = fioAddress;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class regdomain : ITransactionData 
    {
        public string fioDomain;
        public string ownerFioPublicKey;
        public int maxFee;
        public string tpid;
        public string actor;

        public regdomain(string fioDomain, string ownerFioPublicKey, int maxFee, string tpid, string actor)
        {
            this.fioDomain = fioDomain;
            this.ownerFioPublicKey = ownerFioPublicKey;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_domain", fioDomain }, { "owner_fio_public_key", ownerFioPublicKey }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class renewdomain : ITransactionData 
    {
        public string fioDomain;
        public int maxFee;
        public string tpid;
        public string actor;

        public renewdomain(string fioDomain, int maxFee, string tpid, string actor)
        {
            this.fioDomain = fioDomain;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_domain", fioDomain }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class setdomainpub : ITransactionData 
    {
        public string fioDomain;
        public int isPublic;
        public int maxFee;
        public string tpid;
        public string actor;

        public setdomainpub(string fioDomain, int isPublic, int maxFee, string tpid, string actor)
        {
            this.fioDomain = fioDomain;
            this.isPublic = isPublic;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_domain", fioDomain }, { "is_public", isPublic }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class xferdomain : ITransactionData 
    {
        public string fioDomain;
        public string newOwnerFioPublicKey;
        public int maxFee;
        public string tpid;
        public string actor;

        public xferdomain(string fioDomain, string newOwnerFioPublicKey, int maxFee, string tpid, string actor)
        {
            this.fioDomain = fioDomain;
            this.newOwnerFioPublicKey = newOwnerFioPublicKey;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_domain", fioDomain }, { "new_owner_fio_public_key", newOwnerFioPublicKey }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class addnft : ITransactionData 
    {
        public string fioAddress;
        public List<object> nfts;
        public int maxFee;
        public string actor;
        public string tpid;

        public addnft(string fioAddress, List<object> nfts, int maxFee, string actor, string tpid)
        {
            this.fioAddress = fioAddress;
            this.nfts = nfts;
            this.maxFee = maxFee;
            this.actor = actor;
            this.tpid = tpid;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "nfts", nfts }, { "max_fee", maxFee }, { "actor", actor }, { "tpid", tpid }
            };
        }
    }
          
    public class remnft : ITransactionData 
    {
        public string fioAddress;
        public List<object> nfts;
        public int maxFee;
        public string actor;
        public string tpid;

        public remnft(string fioAddress, List<object> nfts, int maxFee, string actor, string tpid)
        {
            this.fioAddress = fioAddress;
            this.nfts = nfts;
            this.maxFee = maxFee;
            this.actor = actor;
            this.tpid = tpid;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "nfts", nfts }, { "max_fee", maxFee }, { "actor", actor }, { "tpid", tpid }
            };
        }
    }
          
    public class remallnfts : ITransactionData 
    {
        public string fioAddress;
        public int maxFee;
        public string actor;
        public string tpid;

        public remallnfts(string fioAddress, int maxFee, string actor, string tpid)
        {
            this.fioAddress = fioAddress;
            this.maxFee = maxFee;
            this.actor = actor;
            this.tpid = tpid;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "max_fee", maxFee }, { "actor", actor }, { "tpid", tpid }
            };
        }
    }
          
    public class stakefio : ITransactionData 
    {
        public int amount;
        public string fioAddress;
        public int maxFee;
        public string tpid;
        public string actor;

        public stakefio(int amount, string fioAddress, int maxFee, string tpid, string actor)
        {
            this.amount = amount;
            this.fioAddress = fioAddress;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "amount", amount }, { "fio_address", fioAddress }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class unstakefio : ITransactionData 
    {
        public int amount;
        public string fioAddress;
        public int maxFee;
        public string tpid;
        public string actor;

        public unstakefio(int amount, string fioAddress, int maxFee, string tpid, string actor)
        {
            this.amount = amount;
            this.fioAddress = fioAddress;
            this.maxFee = maxFee;
            this.tpid = tpid;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "amount", amount }, { "fio_address", fioAddress }, { "max_fee", maxFee }, { "tpid", tpid }, { "actor", actor }
            };
        }
    }
          
    public class voteproducer : ITransactionData 
    {
        public List<object> producers;
        public string fioAddress;
        public string actor;
        public int maxFee;

        public voteproducer(List<object> producers, string fioAddress, string actor, int maxFee)
        {
            this.producers = producers;
            this.fioAddress = fioAddress;
            this.actor = actor;
            this.maxFee = maxFee;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "producers", producers }, { "fio_address", fioAddress }, { "actor", actor }, { "max_fee", maxFee }
            };
        }
    }
          
    public class voteproxy : ITransactionData 
    {
        public string proxy;
        public string fioAddress;
        public string actor;
        public int maxFee;

        public voteproxy(string proxy, string fioAddress, string actor, int maxFee)
        {
            this.proxy = proxy;
            this.fioAddress = fioAddress;
            this.actor = actor;
            this.maxFee = maxFee;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "proxy", proxy }, { "fio_address", fioAddress }, { "actor", actor }, { "max_fee", maxFee }
            };
        }
    }
          
    public class regproxy : ITransactionData 
    {
        public string fioAddress;
        public string actor;
        public int maxFee;

        public regproxy(string fioAddress, string actor, int maxFee)
        {
            this.fioAddress = fioAddress;
            this.actor = actor;
            this.maxFee = maxFee;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "actor", actor }, { "max_fee", maxFee }
            };
        }
    }
          
    public class unregproxy : ITransactionData 
    {
        public string fioAddress;
        public int actor;
        public int maxFee;

        public unregproxy(string fioAddress, int actor, int maxFee)
        {
            this.fioAddress = fioAddress;
            this.actor = actor;
            this.maxFee = maxFee;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "actor", actor }, { "max_fee", maxFee }
            };
        }
    }
          
    public class regproducer : ITransactionData 
    {
        public string fioAddress;
        public string fioPubKey;
        public string url;
        public int location;
        public string actor;
        public int maxFee;

        public regproducer(string fioAddress, string fioPubKey, string url, int location, string actor, int maxFee)
        {
            this.fioAddress = fioAddress;
            this.fioPubKey = fioPubKey;
            this.url = url;
            this.location = location;
            this.actor = actor;
            this.maxFee = maxFee;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "fio_pub_key", fioPubKey }, { "url", url }, { "location", location }, { "actor", actor }, { "max_fee", maxFee }
            };
        }
    }
          
    public class unregprod : ITransactionData 
    {
        public string fioAddress;
        public string actor;
        public int maxFee;

        public unregprod(string fioAddress, string actor, int maxFee)
        {
            this.fioAddress = fioAddress;
            this.actor = actor;
            this.maxFee = maxFee;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "actor", actor }, { "max_fee", maxFee }
            };
        }
    }
          
    public class setfeevote : ITransactionData 
    {
        public List<object> feeRatios;
        public string actor;

        public setfeevote(List<object> feeRatios, string actor)
        {
            this.feeRatios = feeRatios;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fee_ratios", feeRatios }, { "actor", actor }
            };
        }
    }
          
    public class setfeemult : ITransactionData 
    {
        public int multiplier;
        public string actor;

        public setfeemult(int multiplier, string actor)
        {
            this.multiplier = multiplier;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "multiplier", multiplier }, { "actor", actor }
            };
        }
    }
          
    public class computefees : ITransactionData 
    {
        

        public computefees()
        {
            
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                
            };
        }
    }
          
    public class bundlevote : ITransactionData 
    {
        public int bundledTransactions;
        public string actor;

        public bundlevote(int bundledTransactions, string actor)
        {
            this.bundledTransactions = bundledTransactions;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "bundled_transactions", bundledTransactions }, { "actor", actor }
            };
        }
    }
          
    public class tpidclaim : ITransactionData 
    {
        public string actor;

        public tpidclaim(string actor)
        {
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "actor", actor }
            };
        }
    }
          
    public class bpclaim : ITransactionData 
    {
        public string fioAddress;
        public string actor;

        public bpclaim(string fioAddress, string actor)
        {
            this.fioAddress = fioAddress;
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "fio_address", fioAddress }, { "actor", actor }
            };
        }
    }
          
    public class burnexpired : ITransactionData 
    {
        

        public burnexpired()
        {
            
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                
            };
        }
    }
          
    public class burnnfts : ITransactionData 
    {
        public string actor;

        public burnnfts(string actor)
        {
            this.actor = actor;
        }

        public Dictionary<string, object> ToJsonObject() 
        {
            return new Dictionary<string, object>() {
                { "actor", actor }
            };
        }
    }
        
}
