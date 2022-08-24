using System;

namespace FioSharp.Core.DataAttributes
{
    /// <summary>
    /// Data Attribute to map how the field is represented in the abi
    /// </summary>
    public class AbiFieldTypeAttribute : Attribute
    {
        public string AbiType { get; set; }

        public AbiFieldTypeAttribute(string abiType)
        {
            AbiType = abiType;
        }
    }
}
