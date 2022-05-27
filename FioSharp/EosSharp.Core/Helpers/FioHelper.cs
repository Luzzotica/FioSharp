using System;
using System.Collections.Generic;
using EosSharp.Core.Api.v1;

namespace EosSharp.Core.Helpers
{
    public class FioHelper
    {
        public enum FioContentType { newFundsContent, recordObtDataContent }
        public static AbiStruct GetFioAbiStruct(FioContentType contentType)
        {
            if (contentType == FioContentType.newFundsContent)
            {
                return GetNewFundsRequestAbiStruct();
            }
            else if (contentType == FioContentType.recordObtDataContent)
            {
                return GetRecordObtDataContent();
            }

            return GetNewFundsRequestAbiStruct();
        }

        private static AbiStruct newFundsContent = null;
        public static AbiStruct GetNewFundsRequestAbiStruct()
        {
            if (newFundsContent != null)
            {
                return newFundsContent;
            }

            newFundsContent = new AbiStruct();
            newFundsContent.name = "new_funds_content";
            newFundsContent.@base = "";
            newFundsContent.fields = new List<AbiField>();
            newFundsContent.fields.Add(MakeAbiField("payee_public_address", "string"));
            newFundsContent.fields.Add(MakeAbiField("amount", "string"));
            newFundsContent.fields.Add(MakeAbiField("chain_code", "string"));
            newFundsContent.fields.Add(MakeAbiField("token_code", "string"));
            newFundsContent.fields.Add(MakeAbiField("memo", "string?"));
            newFundsContent.fields.Add(MakeAbiField("hash", "string?"));
            newFundsContent.fields.Add(MakeAbiField("offline_url", "string?"));

            return newFundsContent;
        }

        private static AbiStruct recordObtDataContent = null;
        public static AbiStruct GetRecordObtDataContent()
        {
            if (recordObtDataContent != null)
            {
                return recordObtDataContent;
            }

            recordObtDataContent = new AbiStruct();
            recordObtDataContent.name = "new_funds_content";
            recordObtDataContent.@base = "";
            newFundsContent.fields = new List<AbiField>();
            recordObtDataContent.fields.Add(MakeAbiField("payer_public_address", "string"));
            recordObtDataContent.fields.Add(MakeAbiField("payee_public_address", "string"));
            recordObtDataContent.fields.Add(MakeAbiField("amount", "string"));
            recordObtDataContent.fields.Add(MakeAbiField("chain_code", "string"));
            recordObtDataContent.fields.Add(MakeAbiField("token_code", "string"));
            recordObtDataContent.fields.Add(MakeAbiField("status", "string"));
            recordObtDataContent.fields.Add(MakeAbiField("obt_id", "string"));
            recordObtDataContent.fields.Add(MakeAbiField("memo", "string"));
            recordObtDataContent.fields.Add(MakeAbiField("hash", "string"));
            recordObtDataContent.fields.Add(MakeAbiField("offline_url", "string"));

            return recordObtDataContent;
        }

        private static AbiField MakeAbiField(string name, string type)
        {
            AbiField f = new AbiField();
            f.name = name;
            f.type = type;
            return f;
        }
    }
}
