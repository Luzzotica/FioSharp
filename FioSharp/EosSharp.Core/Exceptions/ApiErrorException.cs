using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FioSharp.Core.Exceptions
{
    /// <summary>
    /// Wrapper exception for EOSIO api error
    /// </summary>
    [Serializable]
    public class ApiErrorException : Exception
    {
        public int code { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public List<ApiErrorField> fields { get; set; }

        //public ApiErrorException(int responseCode, Dictionary<string, dynamic> jsonResp)
        //{
        //    code = responseCode;
        //    message = jsonResp.ContainsKey("message") ? jsonResp["message"] : "";
        //    type = jsonResp.ContainsKey("type") ? jsonResp["type"] : "";
        //    fields = jsonResp.ContainsKey("fields") ? jsonResp["fields"] : new List<ApiErrorField>();
        //}

        public ApiErrorException(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                return;
            }

            foreach (SerializationEntry entry in info)
            {
                switch (entry.Name)
                {
                    case "type":
                        type = info.GetString("type");
                        break;
                    case "message":
                        message = info.GetString("message");
                        break;
                    case "fields":
                        fields = (List<ApiErrorField>)info.GetValue("fields", typeof(List<ApiErrorField>));
                        break;
                    default:
                        break;
                }
            }
        }

        public override string ToString()
        {
            return message;
        }
    }

    /// <summary>
    /// EOSIO Api Error
    /// </summary>
    [Serializable]
    public class ApiErrorField
    {
        public string name;
        public string value;
        public string error;
    }
}
