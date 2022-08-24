using System;
namespace FioSharp.Core.Helpers
{
    public class Accounts
    {
        public const string FIO_TOKEN = "fio.token";
        public const string FIO_REQOBT = "fio.reqobt";
        public const string FIO_ADDRESS = "fio.address";
        public const string FIO_STAKING = "fio.staking";
        public const string EOS_IO = "eosio";
        public const string FIO_FEE = "fio.fee";
        public const string FIO_TREASURY = "fio.treasury";

        public class FioToken
        {
            public const string TRNSFIOPUBKY = "trnsfiopubky";
            public const string TRNSLOCTOKS = "trnsloctoks";
        }

        public class FioAddress
        {
            public const string ADDADDRESS = "addaddress";
            public const string REMADDRESS = "remaddress";
            public const string REMALLADDR = "remalladdr";
            public const string REGADDRESS = "regaddress";
            public const string ADDBUNDLES = "addbundles";
            public const string XFERADDRESS = "xferaddress";
            public const string BURNADDRESS = "burnaddress";
            public const string REGDOMAIN = "regdomain";
            public const string RENEWDOMAIN = "renewdomain";
            public const string SETDOMAINPUB = "setdomainpub";
            public const string XFERDOMAIN = "xferdomain";
            public const string ADDNFT = "addnft";
            public const string REMNFT = "remnft";
            public const string REMALLNFTS = "remallnfts";
            public const string BURNEXPIRED = "burnexpired";
            public const string BURNNFTS = "burnnfts";
        }

        public class FioReqObt
        {
            public const string NEWFUNDSREQ = "newfundsreq";
            public const string CANCELFNDREQ = "cancelfndreq";
            public const string REJECTFNDREQ = "rejectfndreq";
            public const string RECORDOBT = "recordobt";
        }

        public class FioStaking
        {
            public const string STAKEFIO = "stakefio";
            public const string UNSTAKEFIO = "unstakefio";
        }

        public class EOSIO
        {
            public const string VOTEPRODUCER = "voteproducer";
            public const string VOTEPROXY = "voteproxy";
            public const string REGPROXY = "regproxy";
            public const string UNREGPROXY = "unregproxy";
            public const string REGPRODUCER = "regproducer";
            public const string UNREGPROD = "unregprod";
        }

        public class FioFee
        {
            public const string SETFEEVOTE = "setfeevote";
            public const string SETFEEMULT = "setfeemult";
            public const string COMPUTEFEES = "computefees";
            public const string BUNDLEVOTE = "bundlevote";
        }

        public class FioTreasury
        {
            public const string TPIDCLAIM = "tpidclaim";
            public const string BPCLAIM = "bpclaim";
        }

    }
    
}
